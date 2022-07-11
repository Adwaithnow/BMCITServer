using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BMCIT.Models.User;

namespace BMCIT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IChartService ChartService = null;
        private readonly ITrainService trainData;
        private readonly ITrainRouteService trainRoute;
        private readonly IStationService stationService;
        public TestController(IChartService chartService, ITrainService TrainData, ITrainRouteService TrainRoute, IStationService StationService)
        {
            ChartService = chartService;
            trainData = TrainData;
            trainRoute = TrainRoute;
            stationService = StationService;
        }
        public IEnumerable<Charts> MyChart => JsonConvert.DeserializeObject<List<Charts>>(System.IO.File.ReadAllText("Databases/Charts.json"));
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Test Is Workings");
        }
        ///////////Chart Services///////////
        [HttpPost("SearchTrain")]
        public IActionResult GetAllCharts(TrainSearch model)
        {
            string fmst = model.FromStation;
            string tsts = model.ToStation;
            string date = model.date;

            Response fromang = stationService.GetStationById(model.FromStation);
            Response toang = stationService.GetStationById(model.ToStation);
            if (model.FromStation == model.ToStation)
            {
                return StatusCode(405, new Response
                {
                    ResCode = 405,
                    RData = "You dont need Train!"
                });
            }
            string ForAngularFromStation = (fromang.RData).StationName;
            string ForAngularTo = (toang.RData).StationName;
            Console.WriteLine(ForAngularFromStation);
            Console.WriteLine(ForAngularTo);
            double? StartStation = null;
            double? StopStation = null;
            IEnumerable<Train> AllTrain = trainData.GetAllTrains;
            IEnumerable<Routes> AllRoute = trainRoute.GetAllRoutes;
            IEnumerable<Charts> AllMycharts = MyChart;

            List<Routes> SearchRoute = new List<Routes>();// [] != null
            Response res = new Response();
            foreach (Routes item in AllRoute)
            {
                // Console.WriteLine(item.Train_Id);
                // double fromexist=(from x in item.Stations where x.StationId==model.FromStation select x.Distance).FirstOrDefault();
                // double toexist=(from x in item.Stations where x.StationId==model.ToStation select x.Distance).FirstOrDefault();
                // Console.WriteLine("From Station" + item.Train_Id + fromexist);
                // Console.WriteLine( + toexist);
                //  x.StationId.Containsmodel.FromStation

                // double? FromStationDS = (from x in item.Stations where x.StationId.Contains(model.FromStation) select x.Distance)?.FirstOrDefault();
                // double? DestStationDS = (from x in item.Stations where x.StationId.Contains(model.ToStation) select x.Distance)?.FirstOrDefault();
                bool FromStationok=item.Stations.Any(x=>x.StationId.Contains(model.FromStation));
                bool DestStationOk=item.Stations.Any(x=>x.StationId.Contains(model.ToStation));
                double? FromStationDS = (from x in item.Stations where x.StationId==model.FromStation select x.Distance)?.FirstOrDefault();
                double? DestStationDS = (from x in item.Stations where x.StationId==model.ToStation select x.Distance)?.FirstOrDefault();
                // true true -> false
                // false false -> true
                // true false -> true
                if (!FromStationok || !DestStationOk) {
                    continue;
                }
                Console.WriteLine(DestStationDS.ToString() + " " + FromStationDS.ToString());
                Console.WriteLine(DestStationDS > FromStationDS);
                if (DestStationDS > FromStationDS)
                {
                    Console.WriteLine("From Station " +" distance :"+ FromStationDS);
                    Console.WriteLine("To Station " +" distance :"+ DestStationDS);

                    StartStation = FromStationDS;
                    StopStation = DestStationDS;
                    SearchRoute.Add(item);
                }
                Console.WriteLine("-----------------------------------------------------------");

            }
            Console.WriteLine(JsonConvert.SerializeObject(SearchRoute));
            Console.WriteLine(SearchRoute.Count.ToString() + " ccc");
            Console.WriteLine("-----------------------------------------------------------");
            if (SearchRoute.Count > 0)
            {
                List<Routes> newroutes = new List<Routes>();
                for (int i = 0; i < AllRoute.Count(); i++)
                {
                    AllRoute.ElementAt(i).Stations = AllRoute.ElementAt(i).Stations.OrderBy(x => x.Distance).ToList();
                }
                // var tester=AllRoute.Sta.OrderBy(x=>x.Stations)
                // Console.WriteLine(JsonConvert.SerializeObject(AllRoute));
                // Console.WriteLine("ith");
                // List<string> hmm=new List<string>();
                // var ok=from x in SearchRoute join c in MyChart on x.Train_Id equals c.Train_Id
                // select new{
                //      RId = x.RId,
                //      Chart_Id=c.Chart_Id,
                //      cStations=c.Stations,
                //      Train_Id=x.Train_Id,
                //      Stations=x.Stations,
                // };
                // Dictionary<string,List<string>> veri=new Dictionary<string,List<string>>();
                // var resd = ok.ToDictionary(x => x.Train_Id, x => x.Stations);
                // // ok.ToList().ForEach(x=>x.Stations.ForEach(k=>hmm.Add(k.StationId)));
                // foreach (var item in ok)
                // {
                //     List<string> stta=new List<string>();
                //     foreach (var items in item.Stations)
                //     {
                //         stta.Add(items.StationId);
                //         Console.WriteLine(items.StationId);
                //     }
                //     veri.Add(item.Train_Id,stta);
                //     // stta.Clear();
                // }


                // res.RData=ok;
                // res.ResCode=200;
                try
                {
                    var datas = from c in AllMycharts where Convert.ToDateTime(c.date) == Convert.ToDateTime(model.date) select c;
                    // Console.WriteLine(JsonConvert.SerializeObject(datas));
                    // Console.WriteLine("--ok" );
                }
                catch (System.Exception e)
                {
                    res.ResCode = 204;
                    res.RData = "No Train";
                    Console.WriteLine(e);
                    return StatusCode(res.ResCode, res);
                }
                try
                {
                    var data = from c in AllMycharts
                               where Convert.ToDateTime(c.date) == Convert.ToDateTime(model.date)
                               join x in SearchRoute on c.Train_Id equals x.Train_Id
                               join z in AllTrain on x.Train_Id equals z.Train_Id
                               select new
                               {
                                   dateofjourney = model.date,
                                   RId = x.RId,
                                   Chart_Id = c.Chart_Id,
                                   Train_Id = x.Train_Id,
                                   TrainName = z.TrainName,
                                   TrainNo = z.TrainNo,
                                   Stations = x.Stations,
                                   DaysRun = z.DaysRun,
                                   FromStation = model.FromStation,
                                   FromStationAng = ForAngularFromStation,
                                   ToStationAng = ForAngularTo,
                                   DestStation = model.ToStation,
                                   ChartStation = c.Stations,
                               };
                    Console.WriteLine(JsonConvert.SerializeObject(data));
                    if (data.Count() > 0)
                    {
                        res.ResCode = 200;
                        res.RData = data;
                        
                    }
                    else
                    {
                        res.ResCode = 405;
                        res.RData = "NO Train";
                    }
                }
                catch (System.Exception)
                {
                    res.ResCode = 405;
                    res.RData = "NO Train";
                    return StatusCode(res.ResCode, res);
                }
                // Console.WriteLine(JsonConvert.SerializeObject(res));
            }
            return StatusCode(res.ResCode, res);
        }
    }
}
