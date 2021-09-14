using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using Training2.Benchmark;

namespace Training2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenchmarkController : ControllerBase
    {
        

        [HttpGet("BenchmarkAnnouncement")]
        public IActionResult BenchmarkAnnouncement()
        {
            BenchmarkRunner.Run<Benchy>();
            return Ok();
        }

    }
}
