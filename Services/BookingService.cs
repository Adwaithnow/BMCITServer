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
        Response res = new Response();
        public string Bpath = "Databases/BookingDetails.json";
        public BookingService(IChartService chartService)
        {
            ChartService = chartService;
        }
        public IEnumerable<Booking> GetAllBookingForAdmin => JsonConvert.DeserializeObject<List<Booking>>(System.IO.File.ReadAllText(Bpath));

        public IChartService ChartService { get; }

        public Response Book(Booking model)
        {
            IEnumerable<Booking> Booking = GetAllBookingForAdmin.Append(model);
            List<Charts> AllChart = ChartService.GetAllCharts.ToList();
            List<List<int>> seatNos = new List<List<int>>();
            int chartindex = AllChart.FindIndex(x => x.Chart_Id == model.Chart_Id);
            // AllChart[chartindex].Stations
            // List<string> stat=model.StationIds;
            foreach (var item in model.PassengerDetails)
            {
                int s = Int32.Parse(item.SeatNo);
                int j = (s - 1) % 4;
                int i = (s - 1 - j) / 4;
                seatNos.Add(new List<int> { i, j });
            }
            Console.WriteLine(JsonConvert.SerializeObject(seatNos), chartindex);
            Console.WriteLine(JsonConvert.SerializeObject(model));

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
            Console.WriteLine(JsonConvert.SerializeObject(AllChart));
            return res;
            // return WriteBooking(Booking);
        }
        public Response GetAllUpcomingBookingByUserId(string Id)
        {
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
            IEnumerable<Booking> AllBooking = GetAllBookingForAdmin;
            IEnumerable<Booking> booking = from x in AllBooking where x.Train_Id == Id.Train_Id && !x.IsCancelled select x;
            IEnumerable<Booking> bok = from x in booking where Convert.ToDateTime(x.DateOfJourney) == Convert.ToDateTime(Id.Date) select x;
            if (bok != null && bok.Any())
            {
                res.ResCode = 200;
                res.RData = bok;
                return res;
            }
            res.ResCode = 200;
            res.RData = "No Booking Found";
            return res;
        }
        public Response CancelBookingById(string Bid)
        {
            List<Booking> AllBooking = GetAllBookingForAdmin.ToList();
            int index = AllBooking.FindIndex(x => x.Bid == Bid);
            if (index >= 0)
            {
                AllBooking.ElementAt(index).IsCancelled = true;
                Response newRespons = WriteBooking(AllBooking);
                newRespons.RData = newRespons.ResCode != 405 ? "Cancelled Booking" : newRespons.RData;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Booking Found";
            return res;
            throw new NotImplementedException();
        }
        public Response WriteBooking(IEnumerable<Booking> booking)
        {
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