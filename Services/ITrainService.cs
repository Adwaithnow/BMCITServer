using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;


namespace BMCIT.Services
{
    public interface ITrainService
    {
        //Train
        Response AddTrain(Train TrainData);
        Response GetTrainById(string Id);
        Response UpdateTrain(Train train);
        Response DeleteOneTrainById(string Id);
        IEnumerable<Train> GetAllTrains { get; }
        IEnumerable<Train> GetAllTrainsWithoutRoute();
      
    }
}