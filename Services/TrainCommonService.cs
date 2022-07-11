using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Models.User;
using Newtonsoft.Json;

namespace BMCIT.Services
{
    public class TrainCommonService : ITrainCommonService
    {
        private readonly ITrainService trainData;
        private readonly ITrainRouteService trainRoute;
        private readonly IStationService stationService;
        private readonly IChartService chartService;

        public TrainCommonService(ITrainService TrainData, ITrainRouteService TrainRoute, IStationService StationService, IChartService chartService)
        {
            trainData = TrainData;
            trainRoute = TrainRoute;
            stationService = StationService;
            this.chartService = chartService;
        }
        public Response SearchResults(TrainSearch model)
        {
            string fmst = model.FromStation;
            string tsts = model.ToStation;
            string date = model.date;

            Response fromang = stationService.GetStationById(model.FromStation);
            Response toang = stationService.GetStationById(model.ToStation);
            if (model.FromStation == model.ToStation)
            {
                return new Response
                {
                    ResCode = 405,
                    RData = "You dont need Train!"
                };
            }
            string ForAngularFromStation = (fromang.RData).StationName;
            string ForAngularTo = (toang.RData).StationName;
            Console.WriteLine(ForAngularFromStation);
            Console.WriteLine(ForAngularTo);
            double? StartStation = null;
            double? StopStation = null;
            IEnumerable<Train> AllTrain = trainData.GetAllTrains;
            IEnumerable<Routes> AllRoute = trainRoute.GetAllRoutes;
            IEnumerable<Charts> AllMycharts = chartService.GetAllCharts;

            List<Routes> SearchRoute = new List<Routes>();
            Response res = new Response();
            foreach (Routes item in AllRoute)
            {
                double? FromStationDS = (from x in item.Stations where x.StationId.Contains(model.FromStation) select x.Distance)?.FirstOrDefault();
                double? DestStationDS = (from x in item.Stations where x.StationId.Contains(model.ToStation) select x.Distance)?.FirstOrDefault();

                if (DestStationDS > FromStationDS)
                {
                    StartStation = FromStationDS;
                    StopStation = DestStationDS;
                    SearchRoute.Add(item);
                }
            }
            Console.WriteLine(JsonConvert.SerializeObject(SearchRoute));
            if (SearchRoute.Count() != 0)
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
                }
                catch (System.Exception e)
                {
                    res.ResCode = 204;
                    res.RData = "No Train";
                    Console.WriteLine(e);
                    return res;
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
                catch (System.Exception)
                {
                    res.ResCode = 204;
                    res.RData = "NO Train";
                    return res;
                }
            }
            else
            {
                res.ResCode = 405;
                res.RData = "No Train Found";
            }
            return res;
        }
        public Response SearchTrain(string FromStation, string DestStation, string? date)
        {

            IEnumerable<Train> AllTrain = trainData.GetAllTrains;
            IEnumerable<Routes> AllRoute = trainRoute.GetAllRoutes;
            List<Routes> SearchRoute = new List<Routes>();
            Response res = new Response();
            foreach (Routes item in AllRoute)
            {
                //    bool FromStationok=item.Stations.Any(x=>x.StationId.Contains(FromStation));
                //    bool DestStationOk=item.Stations.Any(x=>x.StationId.Contains(DestStation));
                double? FromStationDS = (from x in item.Stations where x.StationId.Contains(FromStation) select x.Distance)?.FirstOrDefault();
                double? DestStationDS = (from x in item.Stations where x.StationId.Contains(DestStation) select x.Distance)?.FirstOrDefault();

                if (DestStationDS > FromStationDS)
                {
                    SearchRoute.Add(item);
                }
            }
            if (SearchRoute == null)
            {
                var data = from x in SearchRoute
                           join z in AllTrain on x.Train_Id equals z.Train_Id
                           select new
                           {
                               date = date,
                               RId = x.RId,
                               Train_Id = x.Train_Id,
                               Stations = x.Stations,
                               TrainName = z.TrainName,
                               DaysRun = z.DaysRun,
                               FromStation = FromStation,
                               DestStation = DestStation
                           };
                if (data.Count() > 0)
                {
                    res.ResCode = 200;
                    res.RData = data;
                    return res;
                }
            }
            res.ResCode = 200;
            res.RData = "No Train Found";
            return res;
            // IEnumerable<Station> AllStation = stationService.GetAllStation;
            // Station From = AllStation.Where(x => x.SId == FromStation).FirstOrDefault();
            // Station To = AllStation.Where(x => x.SId == DestStation).FirstOrDefault();
            // // var mystation = AllTrain.Where(x => SearchRoute.Any(a => a.));

            // throw new NotImplementedException();
        }
        public Response GetAllTrains()
        {
            Response res = new Response();
            IEnumerable<Train> AllTrain = trainData.GetAllTrains;
            List<Train> ForUser = new List<Train>();
            foreach (var item in AllTrain)
            {
                ForUser.Add(new Train
                {
                    Train_Id = item.Train_Id,
                    TrainNo = item.TrainNo,
                    TrainName = item.TrainName,
                    FromStation = GetStationName(item.FromStation),
                    ToStation = GetStationName(item.ToStation),
                    DaysRun = item.DaysRun
                });
            }
            if (ForUser != null)
            {
                res.RData = ForUser;
                res.ResCode = 200;
                return res;
            }
            res.RData = "No Trains";
            res.ResCode = 200;
            return res;

        }
        public Response RouteForAdmin()
        {
            Response res = new Response();
            IEnumerable<Routes> route = trainRoute.GetAllRoutes;
            IEnumerable<Train> train = trainData.GetAllTrains;
            try
            {
                     var response = from t in train
                           join r in route on t.Train_Id equals r.Train_Id
                           select new
                           {
                               RId = r.RId,
                               Train_Id = r.Train_Id,
                               TrainNo = t.TrainNo,
                               TrainName = t.TrainName,
                               FromSatation = t.FromStation,
                               ToStation = t.ToStation,
                               ToStationName = GetStationName(t.ToStation),
                               FromStationName = GetStationName(t.FromStation),
                           };
                              res.RData = response;
                              res.ResCode=200;
                              return res;
            }
            catch (System.Exception)
            {
                res.ResCode=405;
                  res.RData = "No Data";
            return res;
                throw;
            }
        }
        public string GetStationName(string id)
        {
            return stationService.GetAllStation.Where(x => x.SId == id).FirstOrDefault().StationName;
        }
    }
}