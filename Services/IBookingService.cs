using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Models.User;

namespace BMCIT.Services
{
    public interface IBookingService
    {
        Response Book(Booking model);
        IEnumerable<Booking> GetAllBookingForAdmin{ get; }
        // Response Cancel(string bookingId);
        Response GetAllUpcomingBookingByUserId(string Id);
        Response GetAllCompletedBookingByUserId(string Id);
        Response GetAllCancelledBookingByUserId(string Id);
        Response GetAllBookingByTrainId(AdminGetBookingByTrainId Id);
        Response CancelBookingById(string Bid);
        // Response CancelBookingTrainId(string TrainId);

    }
}