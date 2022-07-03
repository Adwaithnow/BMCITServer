﻿using System;
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
    public class ChartController : ControllerBase
    {
        private readonly IChartService ChartService=null;
        public ChartController(IChartService chartService)
        {
            ChartService = chartService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Chart Is Workings");
        }       
        ///////////Chart Services///////////
        [HttpGet("GetAllCharts")]
        public IActionResult GetAllCharts()
        {
            return Ok(ChartService.GetAllCharts);
        } 
         [HttpPost("AddChart")]
        public IActionResult AddChart(Charts chartsData)
        {
            chartsData.Chart_Id=Guid.NewGuid().ToString();
            Response res=ChartService.AddChart(chartsData);
            return StatusCode(res.ResCode,res.RData);
        }
      
        [HttpGet("GetOneChartById/{Id}")]
        public IActionResult GetChartById(string Id)
        {
            Response res=ChartService.GetChartById(Id);
            return StatusCode(res.ResCode,res.RData);
        }
        [HttpPatch("UpdateChart")]
        public IActionResult UpdateChart(Charts chartsData)
        {
            Response res=ChartService.UpdateChart(chartsData);
            return StatusCode(res.ResCode,res.RData);
        }
        [HttpDelete("DeleteOneChartById/{Id}")]
        public IActionResult DeleteOneChartById(string Id)
        {
            Response res=ChartService.DeleteOneChartById(Id);
            return StatusCode(res.ResCode,res.RData);
        }      
    }
}
