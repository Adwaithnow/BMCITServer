using System.Collections.Generic;
using System.ComponentModel;
using BMCIT.Models;
namespace BMCIT.Models
{
    public class Booking
    {
        public string Bid { get; set; }
        public string Chart_Id { get; set; }
        public string Id { get; set; }
        public string Train_Id { get; set; }
        public int TrainNo { get; set; }
        public string TrainName { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string FromStationName { get; set; }
        public string ToStationName { get; set; }
        public List<string> StationIds { get; set; }
        [DefaultValue(false)]
        public bool IsCancelled { get; set; }=false;
        public string DateOfBooking { get; set; }
        public string DateOfJourney{ get; set; }
        public string TimeOfJourney{ get; set; }
        public int NoOfPassengers { get; set; }
        public List<Passengers> PassengerDetails { get; set; }
        public string CoachName { get; set; }
        public string CoachType { get; set; }
        public double Fare { get; set; }
    }
        public class Passengers
        {
            public string Name { get; set; }
            public string Gender { get; set; }
            public int Age { get; set; }
            public int SeatNo { get; set; }
        }
}