using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMCIT.Models
{
    
        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Klstation
    {
      
        public string sl{ get; set; }
        public string Code { get; set; }

        public string StationName { get; set; }
        public string Place { get; set; }
    }
    
}