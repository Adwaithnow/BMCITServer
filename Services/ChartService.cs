using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using Newtonsoft.Json;
namespace BMCIT.Services
{
    public class ChartService : IChartService
    {
        //////////////////////////////////////
        ////////////Chart Services////////////
        //////////////////////////////////////
        public string Cpath = "Databases/Charts.json";
        public Response res = new Response();
        public IEnumerable<Charts> GetAllCharts => JsonConvert.DeserializeObject<List<Charts>>(System.IO.File.ReadAllText(Cpath));
        public Response AddChart(Charts station)
        {
            IEnumerable<Charts> olddta = GetAllCharts.Append(station);
            return WriteChart(olddta);
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