using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BMCIT.Models.User
{
    public class BookingDto
    {
        public string Id { get; set; } 
        public string RId { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public int NoOfPassengers { get; set; }
        public string SeatNo { get; set; }
        public string CoachName { get; set; }
        public double Fare { get; set; }
    }
}