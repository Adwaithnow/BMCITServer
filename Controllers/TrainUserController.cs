using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Models.User;
using BMCIT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BMCIT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainUserController : ControllerBase
    {
        private readonly ITrainCommonService TrainService = null;
        private readonly ITrainRouteService routeService;

        public  TrainUserController(ITrainCommonService trainService,ITrainRouteService routeService)
        {
            TrainService = trainService;
            this.routeService = routeService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Account Is Workings");
        }
        ///////////Train Services///////////

       
        [HttpPost("SearchTrain")]
        public IActionResult SearchTrain(TrainSearch data)
        {
            // Console.WriteLine(data.FromStation);
            // Console.WriteLine(data.ToStation);
            // Console.WriteLine(data.date);

            // return Ok(data.FromStation);
            Response res=TrainService.SearchTrain(data.FromStation,data.ToStation,data.date);
            // Console.WriteLine(res.RData);
            return StatusCode(res.ResCode,res.RData);
        }
        [HttpGet("GetAllTrains")]
        public IActionResult GetAllTrains()
        {
            // Console.WriteLine(data.FromStation);
            // Console.WriteLine(data.ToStation);
            // Console.WriteLine(data.date);

            // return Ok(data.FromStation);
            Response res=TrainService.GetAllTrains();
            // Console.WriteLine(res.RData);
            return StatusCode(res.ResCode,res.RData);
        }
        [HttpGet("GetAllRoutesAdmin")]
        public IActionResult GetAllRoutesAdmin()
        {
            // Console.WriteLine(data.FromStation);
            // Console.WriteLine(data.ToStation);
            // Console.WriteLine(data.date);

            // return Ok(data.FromStation);
            Response res=TrainService.RouteForAdmin();
            // Console.WriteLine(res.RData);
            return StatusCode(res.ResCode,res.RData);
        }
        [HttpGet("GetTrainsForRoute")]
          public IActionResult GetTrainsForRoute()
        {
            IEnumerable<Routes> AllRoute=routeService.GetAllRoutes;
            IEnumerable<Train> AllTRain=(IEnumerable<Train>)TrainService.GetAllTrains(); 
            // var data=AllRoute.Where(c => !db.Blacklists.Select(b => b.CusId).Contains(c.CusId));
            Response res=TrainService.GetAllTrains();
            return StatusCode(res.ResCode,res.RData);
        }
        
       
    }
}
