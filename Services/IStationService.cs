using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
namespace BMCIT.Services
{
    public interface IStationService
    {
        //Station
        Response Search(string stationName);
        Response AddStation(Station stationData);
        Response GetStationById(string Id);
        Response UpdateStation(Station stationData);
        Response DeleteOneStationById(string Id);
        IEnumerable<Station> GetAllStation { get; }
    }
}