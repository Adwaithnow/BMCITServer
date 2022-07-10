using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Models.User;
using Newtonsoft.Json;
namespace BMCIT.Services
{
    public class ChartService : IChartService
    {
        //////////////////////////////////////
        ////////////Chart Services////////////
        //////////////////////////////////////
        private readonly ITrainRouteService routeService;
        public ChartService(ITrainRouteService routeService)
        {
            this.routeService = routeService;
        }
        public string Cpath = "Databases/Charts.json";
        public Response res = new Response();

        public IEnumerable<Charts> GetAllCharts => JsonConvert.DeserializeObject<List<Charts>>(System.IO.File.ReadAllText(Cpath));
        public Response AddChart(AdminGetBookingByTrainId trainch)
        {
            //Addign Train Id
            //Getting Trains Route
            Console.WriteLine(JsonConvert.SerializeObject(trainch));
            Charts chartsData = new Charts();
            chartsData.Chart_Id = Guid.NewGuid().ToString();
            chartsData.Train_Id = trainch.Train_Id;
            chartsData.date=trainch.Date;
            // skjfksdjfksdjfsjfksdjklsdjfsjd
            Routes thisTrainRoute = (from x in routeService.GetAllRoutes where x.Train_Id == trainch.Train_Id select x).FirstOrDefault();
            //Getting Train passing station details

           try
           {
             List<RouteSub> TrainStations = thisTrainRoute.Stations;
            List<string> stationid = new List<string>();
            //Finding Station Ids that train passing through

            foreach (var item in TrainStations)
            {
                stationid.Add(item.StationId);
            }

            //Creating Empty Chart Station List and Compartment list and Empty seats
            List<ChartStation> Stations = new List<ChartStation>();
            List<Compartment> compartmentes = new List<Compartment>();
            List<List<int>> Seats = new List<List<int>> { };
            Seats.AddRange(Enumerable.Repeat((new List<int> { 0, 0, 0, 0 }), 10));
            //Ac Compartment
            for (int i = 0; i < 3; i++)
            {
                Compartment ac = new Compartment
                {
                    type = "AC",
                    name = "A" + (i + 1),
                    seats = Seats
                };
                Compartment sl = new Compartment
                {
                    type = "SLEEPER",
                    name = "SL" + (i + 1),
                    seats = Seats
                };
                compartmentes.Add(ac);
                compartmentes.Add(sl);

            }
            // Creating compartment
            foreach (var stid in stationid)
            {
                ChartStation temp = new ChartStation()
                {
                    SId = stid,
                    compartments = compartmentes
                };
                Stations.Add(temp);
            }

            //Assigning value Tetsing
            chartsData.Stations = Stations;
            // Console.WriteLine(JsonConvert.SerializeObject(Stations));
            IEnumerable<Charts> olddta = GetAllCharts.Append(chartsData);
            // IEnumerable<Charts> olddta = GetAllCharts;
            return WriteChart(olddta);
           }
           catch (System.Exception)
           {
             return new Response
            {
                ResCode = 405,
                RData = "No Route For the train found or An Error Occured"
            };
            throw;
           }
          
            
        }


        public Response UpdateChart(Charts ChartsData)
        {
            List<Charts> allCharts = GetAllCharts.ToList();
            int index = allCharts.FindIndex(x => x.Chart_Id == ChartsData.Chart_Id);
            if (index >= 0)
            {
                allCharts.ElementAt(index).Train_Id = ChartsData.Train_Id;
                Response newRespons = WriteChart(allCharts);
                newRespons.RData = newRespons.ResCode != 405 ? "Train chart has been Updated!" : newRespons.RData;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Chart Found";
            return res;
        }
        public Response GetChartById(string Id)
        {
            Charts data = GetAllCharts.Where(x => x.Chart_Id == Id)?.FirstOrDefault();
            if (data != null)
            {
                res.ResCode = 200;
                res.RData = data;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No schedule Found";
            return res;
        }
        public Response DeleteOneChartById(string Id)
        {
            List<Charts> AllCharts = GetAllCharts.ToList();
            int index = AllCharts.FindIndex(x => x.Chart_Id == Id);
            if (index >= 0)
            {
                AllCharts.RemoveAt(index);
                Response newRespons = WriteChart(AllCharts);
                newRespons.RData = newRespons.ResCode != 405 ? "Chart Has been Deleted!" : newRespons.RData;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Chart Found";
            return res;
        }
        public Response WriteChartList(List<Charts> chartData)
        {
            try
            {
                System.IO.File.WriteAllText(Cpath, JsonConvert.SerializeObject(chartData, Formatting.Indented));
            }
            catch (System.Exception)
            {
                res.ResCode = 405;
                res.RData = "An Error Occured !";
                return res;
            }
            res.ResCode = 201;
            res.RData = "Train Charted Succesfully!";
            return res;
        }
        public Response WriteChart(IEnumerable<Charts> chartData)
        {
            try
            {
                System.IO.File.WriteAllText(Cpath, JsonConvert.SerializeObject(chartData, Formatting.Indented));
            }
            catch (System.Exception)
            {
                res.ResCode = 405;
                res.RData = "An Error Occured !";
                return res;
            }
            res.ResCode = 201;
            res.RData = "Train Charted Succesfully!";
            return res;
        }
    }
}