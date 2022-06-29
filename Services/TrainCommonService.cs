using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;

namespace BMCIT.Services
{
    public class TrainCommonService : ITrainCommonService
    {
        private readonly ITrainService trainData;
        private readonly ITrainRouteService trainRoute;
        private readonly IStationService stationService;

        public TrainCommonService(ITrainService TrainData, ITrainRouteService TrainRoute, IStationService StationService)
        {
            trainData = TrainData;
            trainRoute = TrainRoute;
            stationService = StationService;
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
            if (SearchRoute != null)
            {
                var data = from x in SearchRoute
                           join z in AllTrain on x.Train_Id equals z.Train_Id
                           select new
                           {
                               RId = x.RId,
                               Train_Id = x.Train_Id,
                               Stations = x.Stations,
                               TrainName = z.TrainName,
                               DaysRun = z.DaysRun,
                               FromStation = z.FromStation,
                               DestStation = z.ToStation
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
        public string GetStationName(string id)
        {
            return stationService.GetAllStation.Where(x => x.SId == id).FirstOrDefault().StationName;
        }
    }
}