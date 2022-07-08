using System;
using System.Collections.Generic;
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
    public class TrainController : ControllerBase
    {
        private readonly ITrainService TrainService = null;
        public TrainController(ITrainService trainService)
        {
            TrainService = trainService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Account Is Workings");
        }
        ///////////Train Services///////////
        [HttpPost("AddTrain")]
        public IActionResult AddTrain([FromBody]Train Train)
        {
            // Console.WriteLine("TrainName:"+Train.TrainName);
            Train.Train_Id = Guid.NewGuid().ToString();
            Response res = TrainService.AddTrain(Train);
            return StatusCode(res.ResCode, res.RData);
            // return Ok(Train);
        }
        [HttpGet("GetAllTrains")]
        public IActionResult GetAllTrains()
        {
            return Ok(TrainService.GetAllTrains);
        }
        [HttpGet("GetAllTrainsWithoutRoute")]
        public IActionResult GetAllTrainsWithoutRoute()
        {
            return Ok(TrainService.GetAllTrainsWithoutRoute());
        }
         [HttpGet("GetAllTrainsH")]
        public IActionResult GetAllTrainsH()
        {
            return Ok(TrainService.GetAllTrains);
        }
        [HttpGet("GetOneTrainById/{Id}")]
        public IActionResult GetOneTrainById(string Id)
        {
            Response res = TrainService.GetTrainById(Id);
            return StatusCode(res.ResCode, res.RData);
        }
        [HttpPatch("UpdateTrain")]
        public IActionResult UpdateOneTrain(Train traindata)
        {
            Response res = TrainService.UpdateTrain(traindata);
            return StatusCode(res.ResCode, res.RData);
        }
        [HttpDelete("DeleteOneTrainById/{Id}")]
        public IActionResult DeleteOneTrainById(string Id)
        {
            Response res = TrainService.DeleteOneTrainById(Id);
            return StatusCode(res.ResCode, res.RData);
        }
    }
}
