using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
namespace BMCIT.Services
{
    public interface IBookingService
    {
        Response Book(Booking model);
        IEnumerable<Booking> GetAllBookingForAdmin{ get; }
        Response GetAllUpcomingBookingByUserId(string Id);
        Response GetAllCompletedBookingByUserId(string Id);
        Response GetAllCancelledBookingByUserId(string Id);
        Response CancelBookingById(string Bid);
        // Response CancelBookingTrainId(string TrainId);

    }
}