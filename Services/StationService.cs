using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using Newtonsoft.Json;
namespace BMCIT.Services
{
    public class StationService :IStationService
    {
         //////////////////////////////////////
        ///////////Station Services///////////
        //////////////////////////////////////
        public Response res = new Response();
        public string Spath = "Databases/Stations.json";
        public IEnumerable<Station> GetAllStation => JsonConvert.DeserializeObject<List<Station>>(System.IO.File.ReadAllText(Spath));
        public Response Search(string stationName){
            IQueryable<Station> allstation=GetAllStation.AsQueryable();
            if(!string.IsNullOrEmpty(stationName)){
                allstation=allstation.Where(x=>x.StationName.ToLower().Contains(stationName)||x.StationShortCode==stationName.ToUpper());
                res.RData=allstation;
                res.ResCode=200;
                return res;
            }
            res.ResCode=404;
            res.RData="No Station found";
            return res;
        }

        public Response AddStation(Station station)
        {
            IEnumerable<Station> olddta = GetAllStation.Append(station);
            return WriteStation(olddta);
        }
        public Response UpdateStation(Station stationData)
        {
            List<Station> allstation = GetAllStation.ToList();
            int index = allstation.FindIndex(x => x.SId == stationData.SId);
            if (index >= 0)
            {
                allstation.ElementAt(index).StationName = stationData.StationName;
                allstation.ElementAt(index).StationShortCode = stationData.StationShortCode;
                allstation.ElementAt(index).StationLocation = stationData.StationLocation;
                Response newRespons = WriteStation(allstation);
                newRespons.RData = newRespons.ResCode != 405 ? "Station with Name :" + stationData.StationName + " has been Updated!" : newRespons.RData;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Train Found";
            return res;
        }
        public Response GetStationById(string Id)
        {
            Station data = GetAllStation.Where(x => x.SId == Id)?.FirstOrDefault();
            if (data != null)
            {
                res.ResCode = 200;
                res.RData = data;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Station Found";
            return res;
        }
        public Response DeleteOneStationById(string Id)
        {
            List<Station> AllStation = GetAllStation.ToList();
            int index = AllStation.FindIndex(x => x.SId == Id);
            if (index >= 0)
            {
                string StationName = AllStation[index].StationName;
                AllStation.RemoveAt(index);
                Response newRespons = WriteStation(AllStation);
                newRespons.RData = newRespons.ResCode != 405 ? "Train with Name :" + StationName + " Hase been Deleted!" : newRespons.RData;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Station Found";
            return res;
        }
         public Response WriteStation(IEnumerable<Station> stations)
        {
            try
            {
                System.IO.File.WriteAllText(Spath, JsonConvert.SerializeObject(stations, Formatting.Indented));
            }
            catch (System.Exception)
            {
                res.ResCode = 405;
                res.RData = "An Error Occured !";
                return res;
            }
            res.ResCode = 201;
            res.RData = "Station added successfully !";
            return res;
        }
    }
}