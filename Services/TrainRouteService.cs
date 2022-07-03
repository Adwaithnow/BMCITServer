using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using Newtonsoft.Json;
namespace BMCIT.Services
{
    public class TrainRouteService : ITrainRouteService
    {
        /////////////////////////////////////////
        ///////////TrainRoute Services///////////
        /////////////////////////////////////////
        public string TRpath = "Databases/TrainRoutes.json";
        public Response res = new Response();
        public IEnumerable<Routes> GetAllRoutes => JsonConvert.DeserializeObject<List<Routes>>(System.IO.File.ReadAllText(TRpath));
        public Response GetRouteById(string Id)
        {
            Routes data = GetAllRoutes.Where(x => x.RId == Id)?.FirstOrDefault();
            if (data != null)
            {
                res.ResCode = 200;
                res.RData = data;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Route Found";
            return res;
        }
          public Response GetRouteByTrainId(string Id)
        {
            Routes data = GetAllRoutes.Where(x => x.Train_Id == Id)?.FirstOrDefault();
            if (data != null)
            {
                res.ResCode = 200;
                res.RData = data;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Route Found";
            return res;
        }
        public Response DeleteOneRouteById(string Id)
        {
            List<Routes> AllRoute = GetAllRoutes.ToList();
            int index = AllRoute.FindIndex(x => x.RId == Id);
            if (index >= 0)
            {
                AllRoute.RemoveAt(index);
                Response newRespons = WriteRoute(AllRoute);
                newRespons.RData = newRespons.ResCode != 405 ? "Route has been Deleted!" : newRespons.RData;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Route Found";
            return res;
        }
        public Response AddRoute(Routes routesData)
        {
            IEnumerable<Routes> olddta = GetAllRoutes.Append(routesData);
            return WriteRoute(olddta);
        }
        public Response UpdateRoute(Routes routesData)
        {
            List<Routes> AllRoute = GetAllRoutes.ToList();
            int index = AllRoute.FindIndex(x => x.RId == routesData.RId);
            if (index >= 0)
            {
                AllRoute[index].Stations = routesData.Stations;
                Response newRespons = WriteRoute(AllRoute);
                newRespons.RData = newRespons.ResCode != 405 ? "Route been Updated!" : newRespons.RData;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Train Found";
            return res;
        }
        public Response WriteRoute(IEnumerable<Routes> traindata)
        {
            try
            {
                System.IO.File.WriteAllText(TRpath, JsonConvert.SerializeObject(traindata, Formatting.Indented));
            }
            catch (System.Exception)
            {
                res.ResCode = 405;
                res.RData = "An Error Occured !";
                return res;
            }
            res.ResCode = 201;
            res.RData = "Route added successfully now you can chart it!";
            return res;
        }
    }
}