using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
namespace BMCIT.Services
{
    public interface IChartService
    {
        //Chart
        Response AddChart(Charts chartsData);
        Response GetChartById(string Id);
        Response UpdateChart(Charts chartsData);
        Response DeleteOneChartById(string Id);
        IEnumerable<Charts> GetAllCharts { get; }
    }
}