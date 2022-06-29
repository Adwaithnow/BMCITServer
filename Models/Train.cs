using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMCIT.Models
{
    public class Train
    {
        public string Train_Id { get; set; }
        public int TrainNo { get; set; }
        public string TrainName { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public List<int> DaysRun { get; set; }

    }
}