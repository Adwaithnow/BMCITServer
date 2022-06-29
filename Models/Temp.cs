using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMCIT.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Geometry
    {
        public List<double> coordinates { get; set; }
    }

    public class Properties
    {
        public string state { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string zone { get; set; }
        public string address { get; set; }
    }

    public class Root
    {
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }
}