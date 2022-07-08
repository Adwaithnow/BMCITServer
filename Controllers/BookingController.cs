using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Models.User;
using BMCIT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BMCIT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService bookingService;
        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
            
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Account Is Workings");
        }
        ///////////Train Services///////////
        [HttpPost("AddBooking")]
        public IActionResult AddBooking( Booking book)
        {
            book.Bid = Guid.NewGuid().ToString();
            Response res = bookingService.Book(book);
            return StatusCode(res.ResCode, res.RData);

        }
        [HttpGet("GetAllBookingForAdmin")]
        public IActionResult GetAllBookingForAdmin()
        {
            return Ok(bookingService.GetAllBookingForAdmin);
        }
        [HttpPost("GetAllBookingForAdminByTrainId")]
        public IActionResult GetAllBookingForAdminByTrainId(AdminGetBookingByTrainId data)
        {
            Response res=bookingService.GetAllBookingByTrainId(data);
            Console.WriteLine(data.Date,data.Train_Id);
            return StatusCode(res.ResCode,res.RData);
        }
        [HttpGet("GetAllUpcomingBookingByUserId/{Id}")]
        public IActionResult GetAllUpcomingBookingByUserId(string Id)
        {
            Response Res=bookingService.GetAllUpcomingBookingByUserId(Id);
            return StatusCode(Res.ResCode,Res.RData);
        }
        [HttpGet("GetAllCompletedBookingByUserId/{Id}")]
        public IActionResult GetAllCompletedBookingByUserId(string Id)
        {
            Response Res=bookingService.GetAllCompletedBookingByUserId(Id);
            return StatusCode(Res.ResCode,Res.RData);
        }
        [HttpGet("GetAllCancelledBookingByUserId/{Id}")]
        public IActionResult GetAllCancelledBookingByUserId(string Id)
        {
            Response Res=bookingService.GetAllCancelledBookingByUserId(Id);
            return StatusCode(Res.ResCode,Res.RData);
        }
        [HttpPatch("CancelBookingById")]
        public IActionResult CancelBookingById(string Id)
        {
            Response Res=bookingService.CancelBookingById(Id);
            return StatusCode(Res.ResCode,Res.RData);
        }
        





    }
}
