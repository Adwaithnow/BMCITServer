using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Models.User;

namespace BMCIT.Services
{
    public interface IChartService
    {
        //Chart
        Response AddChart(AdminGetBookingByTrainId chartsData);
        public Response GetAllChartForAdminH();
        Response GetChartById(string Id);
        Response UpdateChart(Charts chartsData);
        Response DeleteOneChartById(string Id);
        IEnumerable<Charts> GetAllCharts { get; }
        public Response WriteChartList(List<Charts> chartData);
    }
}