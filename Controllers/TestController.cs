﻿using System;
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
            // Console.WriteLine(JsonConvert.SerializeObject(MyChart));
            // Console.WriteLine(model.date);
            double? StartStation=null;
            double? StopStation=null;
            IEnumerable<Train> AllTrain = trainData.GetAllTrains;
            IEnumerable<Routes> AllRoute = trainRoute.GetAllRoutes;
            IEnumerable<Charts> AllMycharts = MyChart;

            List<Routes> SearchRoute = new List<Routes>();
            Response res = new Response();
            foreach (Routes item in AllRoute)
            {
                double? FromStationDS = (from x in item.Stations where x.StationId.Contains(model.FromStation) select x.Distance)?.FirstOrDefault();
                double? DestStationDS = (from x in item.Stations where x.StationId.Contains(model.ToStation) select x.Distance)?.FirstOrDefault();

                if (DestStationDS > FromStationDS)
                {
                    StartStation=FromStationDS;
                    StopStation=DestStationDS;
                    SearchRoute.Add(item);
                }
            }
            if (SearchRoute != null)
            {
                List<Routes> newroutes=new List<Routes>();
                for (int i = 0; i < AllRoute.Count(); i++)
                {
                    AllRoute.ElementAt(i).Stations = AllRoute.ElementAt(i).Stations.OrderBy(x=>x.Distance).ToList();
                }
                // var tester=AllRoute.Sta.OrderBy(x=>x.Stations)
                Console.WriteLine(JsonConvert.SerializeObject(AllRoute));
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
                Console.WriteLine(model.FromStation);
                Console.WriteLine(model.ToStation);
                Console.WriteLine(model.date);
               try
               {
                var datas=from c in AllMycharts where Convert.ToDateTime(c.date)== Convert.ToDateTime(model.date) select c;
               }
               catch (System.Exception e)
               {
                Console.WriteLine(e);                
                return Ok(e.Message);
               }
                var data=from c in AllMycharts where Convert.ToDateTime(c.date)== Convert.ToDateTime(model.date) join x in SearchRoute on c.Train_Id equals x.Train_Id  join z in AllTrain on x.Train_Id equals z.Train_Id
                select new
                           {
                               RId = x.RId,
                               Train_Id = x.Train_Id,
                               TrainName = z.TrainName,
                               TrainNo = z.TrainNo,
                               Stations = x.Stations,
                               DaysRun = z.DaysRun,
                               FromStation = z.FromStation,
                               DestStation = z.ToStation,
                               ChartStation=c.Stations,
                           }; 
                // var data = from x in SearchRoute
                //            join z in AllTrain on x.Train_Id equals z.Train_Id
                //            select new
                //            {
                //                RId = x.RId,
                //                Train_Id = x.Train_Id,
                //                TrainName = z.TrainName,
                //                Stations = x.Stations,
                //                DaysRun = z.DaysRun,
                //                FromStation = z.FromStation,
                //                DestStation = z.ToStation,
                //            }; 
                if (data.Count() > 0)
                {
                    res.ResCode = 200;
                    res.RData = data;
                }
                else
                {
                    res.ResCode = 204;
                    res.RData = "NO Train";
                }
            }
            return StatusCode(res.ResCode, res.RData);
        }
    }
}
