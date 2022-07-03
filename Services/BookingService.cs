using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using Newtonsoft.Json;

namespace BMCIT.Services
{
    public class BookingService : IBookingService
    {
        Response res=new Response();
        public string Bpath = "Databases/BookingDetails.json";

        public IEnumerable<Booking> GetAllBookingForAdmin=> JsonConvert.DeserializeObject<List<Booking>>(System.IO.File.ReadAllText(Bpath));
        public Response Book(Booking model)
        {
           IEnumerable<Booking> Booking = GetAllBookingForAdmin.Append(model);
            return WriteBooking(Booking);
        }
        public Response GetAllUpcomingBookingByUserId(string Id)
        {
            IEnumerable<Booking> AllBooking=GetAllBookingForAdmin;
            IEnumerable<Booking> UpcomingBooking=from x in AllBooking where (Convert.ToDateTime(x.DateOfJourney)>=DateTime.Now)&&x.Id==Id&&!x.IsCancelled select x;
            if(UpcomingBooking!=null){
                res.ResCode=200;
                res.RData=UpcomingBooking;
                return res;
            }
            res.ResCode=200;
            res.RData="No Booking Found";
            return res;
        }
         public Response GetAllCompletedBookingByUserId(string Id)
        {
            IEnumerable<Booking> AllBooking=GetAllBookingForAdmin;
            IEnumerable<Booking> UpcomingBooking=from x in AllBooking where (Convert.ToDateTime(x.DateOfJourney)<=DateTime.Now)&&x.Id==Id&&!x.IsCancelled select x;
            if(UpcomingBooking!=null){
                res.ResCode=200;
                res.RData=UpcomingBooking;
                return res;
            }
            res.ResCode=200;
            res.RData="No Booking Found";
            return res;
        }
        public Response GetAllCancelledBookingByUserId(string Id)
        {
            IEnumerable<Booking> AllBooking=GetAllBookingForAdmin;
            IEnumerable<Booking> UpcomingBooking=from x in AllBooking where x.Id==Id&&x.IsCancelled select x;
            if(UpcomingBooking!=null){
                res.ResCode=200;
                res.RData=UpcomingBooking;
                return res;
            }
            res.ResCode=200;
            res.RData="No Booking Found";
            return res;
        }

        public Response CancelBookingById(string Bid)
        {
            List<Booking> AllBooking = GetAllBookingForAdmin.ToList();
            int index = AllBooking.FindIndex(x => x.Bid ==Bid);
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