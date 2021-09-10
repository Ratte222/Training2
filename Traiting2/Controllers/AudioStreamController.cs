using BLL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Traiting2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioStreamController : ControllerBase
    {
        private readonly AppSettings _appSettings;

        public AudioStreamController(AppSettings appSettings)
        {
            (_appSettings) = (appSettings);
        }

        [HttpGet("{name}")]
        public IActionResult GetAudio(string name)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), _appSettings.DefaultPathToVideo, name);//"TargetVideo.mp4"

            Stream video = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(video, "audio/mpeg", enableRangeProcessing: true);
        }
    }
}
