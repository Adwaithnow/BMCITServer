using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BMCIT.Models
{
    public class Routes
    {
        public string RId { get; set; }
        public string Train_Id { get; set; }
        public List<RouteSub> Stations { get; set; }
    }
}