using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using BMCIT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace BMCIT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly ITrainRouteService TrainRouteService=null;
        public RouteController(ITrainRouteService trainRouteService)
        {
            TrainRouteService = trainRouteService;            
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Route Is Workings");
        }       
        ///////////TrainRoute Services///////////

        [HttpGet("GetAllRoutes")]
        public IActionResult GetAllRoutes()
        {
            return Ok(TrainRouteService.GetAllRoutes);
        } 
         [HttpPost("AddRoute")]
        public IActionResult AddRoutes(Routes RoutesData)
        {
            RoutesData.RId=Guid.NewGuid().ToString();
            Response res=TrainRouteService.AddRoute(RoutesData);
            return StatusCode(res.ResCode,res.RData);
        }
        [HttpGet("GetAllRoutess")]
        public IActionResult GetAllRoutess()
        {
            return Ok(TrainRouteService.GetAllRoutes);
        }
        [HttpGet("GetOneRoutesById/{Id}")]
        public IActionResult GetOneRoutesById(string Id)
        {
            Response res=TrainRouteService.GetRouteById(Id);
            return StatusCode(res.ResCode,res.RData);
        }
         [HttpGet("GetOneRoutesByTrainId/{Id}")]
        public IActionResult GetOneRoutesByTrainId(string Id)
        {
            Response res=TrainRouteService.GetRouteByTrainId(Id);
            return StatusCode(res.ResCode,res.RData);
        }
        [HttpPatch("UpdateRoutes")]
        public IActionResult UpdateRoutes(Routes RoutesData)
        {
            Response res=TrainRouteService.UpdateRoute(RoutesData);
            return StatusCode(res.ResCode,res.RData);
        }
        [HttpDelete("DeleteOneRoutesById/{Id}")]
        public IActionResult DeleteOneRoutesById(string Id)
        {
            Response res=TrainRouteService.DeleteOneRouteById(Id);
            return StatusCode(res.ResCode,res.RData);
        }      

    }
}
