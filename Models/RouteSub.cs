using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMCIT.Models
{
    public class RouteSub
    {
        public string StationId { get; set; }
        public int PlatForm { get; set; }
        public string TimeArrival { get; set; }
        public string TimeDeparture { get; set; }
        public double Distance { get; set; }
        public double HaltTime { get; set; }
        public int Day { get; set; }
    }
}