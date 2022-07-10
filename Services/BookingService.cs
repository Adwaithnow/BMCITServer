using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Models.User;
using Newtonsoft.Json;

namespace BMCIT.Services
{
    public class BookingService : IBookingService
    {
        
        public string Bpath = "Databases/BookingDetails.json";
        public IChartService ChartService =null;
        public BookingService(IChartService chartService)
        {
            ChartService = chartService;
        }
        public IEnumerable<Booking> GetAllBookingForAdmin => JsonConvert.DeserializeObject<List<Booking>>(System.IO.File.ReadAllText(Bpath));


        public Response CancelBookingById(string bookingId)
        {
            Response res = new Response();
            List<Booking> Bookings = GetAllBookingForAdmin.ToList();
            int bid = Bookings.FindIndex(x=>x.Bid==bookingId);
            List<List<int>> seatNos = new List<List<int>>();
            string cid=Bookings[bid].Chart_Id;
            Bookings[bid].IsCancelled = true;
            List<Charts> AllChart = ChartService.GetAllCharts.ToList();
            int chartindex = AllChart.FindIndex(x => x.Chart_Id == cid);
            foreach (var item in Bookings[bid].PassengerDetails)
            {
                // int s = Int32.Parse(item.SeatNo);
                int s = item.SeatNo;
                int j = (s - 1) % 4;
                int i = (s - 1 - j) / 4;
                seatNos.Add(new List<int> { i, j });
            }
            foreach (var stationid in Bookings[bid].StationIds)
            {
                int stationidex = AllChart[chartindex].Stations.FindIndex(x => x.SId == stationid);
                int coachindex = AllChart[chartindex].Stations[stationidex].compartments.FindIndex(x => x.name == Bookings[bid].CoachName);
                foreach (var seat in seatNos)
                {
                    AllChart[chartindex].Stations[stationidex].compartments[coachindex].seats[seat[0]][seat[1]] = 0;
                }
            }
             Console.WriteLine("------------------------------------------------------\n");
            // Console.WriteLine(JsonConvert.SerializeObject(AllChart));
            // res.RData=AllChart;
            // res.ResCode=200;
            // return res;
            Response chrtrspn=ChartService.WriteChartList(AllChart);
            Console.WriteLine(JsonConvert.SerializeObject(chrtrspn));
            return res;
        }
        public Response Book(Booking model)
        {
            Response res = new Response();
            IEnumerable<Booking> Booking = GetAllBookingForAdmin.Append(model);
            List<Charts> AllChart = ChartService.GetAllCharts.ToList();
            List<List<int>> seatNos = new List<List<int>>();
            int chartindex = AllChart.FindIndex(x => x.Chart_Id == model.Chart_Id);
            // AllChart[chartindex].Stations
            // List<string> stat=model.StationIds;
            Console.WriteLine(JsonConvert.SerializeObject(model));
            foreach (var item in model.PassengerDetails)
            {
                // int s = Int32.Parse(item.SeatNo);
                int s = item.SeatNo;
                int j = (s - 1) % 4;
                int i = (s - 1 - j) / 4;
                seatNos.Add(new List<int> { i, j });
            }
            Console.WriteLine(JsonConvert.SerializeObject(seatNos), chartindex);

            foreach (var stationid in model.StationIds)
            {
                int stationidex = AllChart[chartindex].Stations.FindIndex(x => x.SId == stationid);
                int coachindex = AllChart[chartindex].Stations[stationidex].compartments.FindIndex(x => x.name == model.CoachName);
                foreach (var seat in seatNos)
                {
                    if (AllChart[chartindex].Stations[stationidex].compartments[coachindex].seats[seat[0]][seat[1]] != 0)
                    {
                        // todo return already booked
                        // res.ResCode = 
                    }
                    AllChart[chartindex].Stations[stationidex].compartments[coachindex].seats[seat[0]][seat[1]] = 1;
                }
            }
            Console.WriteLine("------------------------------------------------------\n");
            // Console.WriteLine(JsonConvert.SerializeObject(AllChart));
            // res.RData=AllChart;
            // res.ResCode=200;
            // return res;
            Response chrtrspn=ChartService.WriteChartList(AllChart);
            Console.WriteLine(JsonConvert.SerializeObject(chrtrspn));
            return WriteBooking(Booking);
        }
        public Response GetAllUpcomingBookingByUserId(string Id)
        {
            Response res = new Response();
            IEnumerable<Booking> AllBooking = GetAllBookingForAdmin;
            IEnumerable<Booking> UpcomingBooking = from x in AllBooking where (Convert.ToDateTime(x.DateOfJourney) >= DateTime.Now) && x.Id == Id && !x.IsCancelled select x;
            if (UpcomingBooking != null)
            {
                res.ResCode = 200;
                res.RData = UpcomingBooking;
                return res;
            }
            res.ResCode = 200;
            res.RData = "No Booking Found";
            return res;
        }
        public Response GetAllCompletedBookingByUserId(string Id)
        {
            Response res = new Response();
            IEnumerable<Booking> AllBooking = GetAllBookingForAdmin;
            IEnumerable<Booking> UpcomingBooking = from x in AllBooking where (Convert.ToDateTime(x.DateOfJourney) <= DateTime.Now) && x.Id == Id && !x.IsCancelled select x;
            if (UpcomingBooking != null)
            {
                res.ResCode = 200;
                res.RData = UpcomingBooking;
                return res;
            }
            res.ResCode = 200;
            res.RData = "No Booking Found";
            return res;
        }

        public Response GetAllCancelledBookingByUserId(string Id)
        {
            Response res = new Response();
            IEnumerable<Booking> AllBooking = GetAllBookingForAdmin;
            IEnumerable<Booking> UpcomingBooking = from x in AllBooking where x.Id == Id && x.IsCancelled select x;
            if (UpcomingBooking != null)
            {
                res.ResCode = 200;
                res.RData = UpcomingBooking;
                return res;
            }
            res.ResCode = 200;
            res.RData = "No Booking Found";
            return res;
        }
        Response IBookingService.GetAllBookingByTrainId(AdminGetBookingByTrainId Id)
        {
            Response res = new Response();
            IEnumerable<Booking> AllBooking = GetAllBookingForAdmin;
            IEnumerable<Booking> booking = from x in AllBooking where x.Train_Id == Id.Train_Id && !x.IsCancelled select x;
            IEnumerable<Booking> bok = from x in booking where Convert.ToDateTime(x.DateOfJourney) == Convert.ToDateTime(Id.Date) select x;
            if (bok != null && bok.Any())
            {
                res.ResCode = 200;
                res.RData = bok;
                return res;
            }
            res.ResCode = 405;
            res.RData = "No Booking Found";
            return res;
        }
        // public Response CancelBookingById(string Bid)
        // {
        //     Response res = new Response();
        //     List<Booking> AllBooking = GetAllBookingForAdmin.ToList();
        //     int index = AllBooking.FindIndex(x => x.Bid == Bid);
        //     if (index >= 0)
        //     {
        //         AllBooking.ElementAt(index).IsCancelled = true;
        //         Response newRespons = WriteBooking(AllBooking);
        //         newRespons.RData = newRespons.ResCode != 405 ? "Cancelled Booking" : newRespons.RData;
        //         return res;
        //     }
        //     res.ResCode = 404;
        //     res.RData = "No Booking Found";
        //     return res;
        //     throw new NotImplementedException();
        // }
        public Response WriteBooking(IEnumerable<Booking> booking)
        {
            Response res = new Response();
            try
            {
                System.IO.File.WriteAllText(Bpath, JsonConvert.SerializeObject(booking, Formatting.Indented));
            }
            catch (System.Exception)
            {
                res.ResCode = 405;
                res.RData = "An Error Occured !";
                return res;
            }
            res.ResCode = 201;
            res.RData = "Booking Saved !";
            return res;
        }
    }
}