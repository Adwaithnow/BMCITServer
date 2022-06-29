using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using Newtonsoft.Json;
namespace BMCIT.Services
{
    public class TrainService : ITrainService
    {
        public string Tpath = "Databases/Trains.json";
        public Response res = new Response();

        ////////////////////////////////////
        ///////////Train Services///////////
        ////////////////////////////////////
        public IEnumerable<Train> GetAllTrains => JsonConvert.DeserializeObject<List<Train>>(System.IO.File.ReadAllText(Tpath));

        public Response GetTrainById(string Id)
        {
            Train data = GetAllTrains.Where(x => x.Train_Id == Id)?.FirstOrDefault();
            if (data != null)
            {
                res.ResCode = 200;
                res.RData = data;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Train Found";
            return res;
        }
        public Response DeleteOneTrainById(string Id)
        {
            List<Train> alltrain = GetAllTrains.ToList();
            int index = alltrain.FindIndex(x => x.Train_Id == Id);
            if (index >= 0)
            {
                int TrainNo = alltrain[index].TrainNo;
                alltrain.RemoveAt(index);
                Response newRespons = WriteTrain(alltrain);
                newRespons.RData = newRespons.ResCode != 405 ? "Train with " + TrainNo + " Hase been Deleted!" : newRespons.RData;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Train Found";
            return res;
        }
        public Response AddTrain(Train TrainData)
        {
            IEnumerable<Train> olddta = GetAllTrains.Append(TrainData);
            return WriteTrain(olddta);
        }
        public Response UpdateTrain(Train TrainData)
        {
            List<Train> alltrain = GetAllTrains.ToList();
            int index = alltrain.FindIndex(x => x.Train_Id == TrainData.Train_Id);
            if (index >= 0)
            {
                alltrain[index].TrainName = TrainData.TrainName != null ? TrainData.TrainName : alltrain[index].TrainName;
                alltrain[index].DaysRun = TrainData.TrainName != null ? TrainData.DaysRun : alltrain[index].DaysRun;
                alltrain[index].TrainNo = TrainData.TrainNo != 0 ? TrainData.TrainNo : alltrain[index].TrainNo;
                alltrain[index].FromStation = TrainData.FromStation != null ? TrainData.FromStation : alltrain[index].FromStation;
                alltrain[index].ToStation = TrainData.ToStation != null ? TrainData.ToStation : alltrain[index].ToStation;
                Response newRespons = WriteTrain(alltrain);
                newRespons.RData = newRespons.ResCode != 405 ? "Train with " + TrainData.TrainNo + " Hase been Updated!" : newRespons.RData;
                return res;
            }
            res.ResCode = 404;
            res.RData = "No Train Found";
            return res;
        }
        /////////////////////////////////////////////////////////////
        ////////////////////////JsonOperation////////////////////////
        /////////////////////////////////////////////////////////////
        public Response WriteTrain(IEnumerable<Train> traindata)
        {
            try
            {
                System.IO.File.WriteAllText(Tpath, JsonConvert.SerializeObject(traindata, Formatting.Indented));
            }
            catch (System.Exception)
            {
                res.ResCode = 405;
                res.RData = "An Error Occured !";
                return res;
            }
            res.ResCode = 201;
            res.RData = "Train added successfully now you can assign schedule it!";
            return res;
        }
    }
}