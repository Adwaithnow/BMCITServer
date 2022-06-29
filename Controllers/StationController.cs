using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BMCIT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationController : ControllerBase
    {
        private readonly IStationService StationService = null;
        public StationController(IStationService stationService)
        {
            StationService = stationService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Station Is Workings");
        }
        ///////////Station Services///////////
        [HttpPost("AddStation")]
        public IActionResult AddStation(Station stationData)
        {
            stationData.SId = Guid.NewGuid().ToString();
            Response res = StationService.AddStation(stationData);
            return StatusCode(res.ResCode, res.RData);
        }
        [HttpPost("Search")]
        public IActionResult SearchStation(string stationName)
        {
            Response res = StationService.Search(stationName.ToLower());
            return StatusCode(res.ResCode, res.RData);
        }
        [HttpGet("GetAllStations")]
        public IActionResult GetAllStations()
        {
            return Ok(StationService.GetAllStation);
        }
        [HttpGet("GetOneStationById/{Id}")]
        public IActionResult GetOneStationById(string Id)
        {
            Response res = StationService.GetStationById(Id);
            return StatusCode(res.ResCode, res.RData);
        }
        [HttpPatch("UpdateStation")]
        public IActionResult UpdateStation(Station stationData)
        {
            Response res = StationService.UpdateStation(stationData);
            return StatusCode(res.ResCode, res.RData);
        }
        [HttpDelete("DeleteOneStationById/{Id}")]
        public IActionResult DeleteOneStationById(string Id)
        {
            Response res = StationService.DeleteOneStationById(Id);
            return StatusCode(res.ResCode, res.RData);
        }
        // [HttpGet("Activate")]
        // public IActionResult GetData(){
        //     List<Station> stations=new List<Station>();
        //     IDictionary<string, string> DicStation = JsonConvert.DeserializeObject<IDictionary<string, string>>(System.IO.File.ReadAllText("Tets.json"));
        //     foreach (KeyValuePair<string, string> kvp in DicStation)
        //     {
        //         Station temp=new Station{
        //             SId=Guid.NewGuid().ToString(),
        //             StationName=kvp.Value,
        //             StationShortCode=kvp.Key
        //         };
        //         stations.Add(temp);
        //     }
        //     return Ok(stations);
        // }
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        [HttpGet("Activate")]
        public IActionResult GetData()
        {
            // List<Root> myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(System.IO.File.ReadAllText("Tets.json"));
            List<Station> AllStations = new List<Station>();
            List<Klstation> klstations =  JsonConvert.DeserializeObject<List<Klstation>>(System.IO.File.ReadAllText("KLStations.json"));
            List<Root> myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(System.IO.File.ReadAllText("Tets.json"));
            // var all = listB.Where(b => listA.Any(a => a.code == b.code));

            var myDeserialized=myDeserializedClass.Where(x=> klstations.Any(a=>a.Code==x.properties.code));
            foreach (var item in myDeserialized)
            {
                Station newstation = new Station
                {
                    SId = Guid.NewGuid().ToString(),
                    StationName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.properties?.name.ToLower()),
                    StationShortCode = item.properties?.code,
                    StationLocation = item.geometry?.coordinates
                };
                AllStations.Add(newstation);
            }
            StationService obj=new StationService();
            obj.WriteStation(AllStations);
            // return Ok("Ok");
            // List<Station> data=JsonConvert.DeserializeObject<List<Station>>(System.IO.File.ReadAllText("Databases/Stations.json"));
            
            // StationService obj=new StationService();
            // obj.WriteStation(data);
            return Ok();
        }

      

    }
}
