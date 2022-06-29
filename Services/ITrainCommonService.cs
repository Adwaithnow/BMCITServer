using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;

namespace BMCIT.Services
{
    public interface ITrainCommonService
    {
        Response SearchTrain(string FromStation,string DestStation,string date);
        Response GetAllTrains();


    }
}