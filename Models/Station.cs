using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BMCIT.Models
{
    public class Station
    {
        public string SId { get; set; }
        public string StationName { get; set; }
        public string StationShortCode { get; set; }
        public List<double> StationLocation { get; set; }
    }
}