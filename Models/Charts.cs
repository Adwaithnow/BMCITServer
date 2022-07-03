using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BMCIT.Models
{
    public class Charts
    {
        public string Chart_Id { get; set; }
        public string Train_Id { get; set; }
        public string date { get; set; }
        public List<ChartStation> Stations { get; set; }
    }

    public class ChartStation
    {
        public string SId { get; set; }
        public List<Compartment> compartments { get; set; }
    }
    public class Compartment
    {
        public string type { get; set; }
        public string name { get; set; }
        public List<List<int>> seats { get; set; }
        public int availilability()
        {
            int count = 0;
            seats.ForEach(delegate (List<int> rows) {
                rows.ForEach(delegate (int col) {
                    if(col == 0) {
                        count++;
                    }
                });
            });
            return count;
        }
    }    
}