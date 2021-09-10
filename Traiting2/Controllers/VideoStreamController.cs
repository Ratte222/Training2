using BLL.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Web.Http;

namespace Traiting2.Controllers
{
    //https://www.c-sharpcorner.com/article/asynchronous-videos-live-streaming-with-asp-net-web-apis-2-0/ do not use
    //https://stackoverflow.com/questions/31766623/stream-video-content-through-web-api-2 //useful
    //https://khalidabuhakmeh.com/partial-range-http-requests-with-aspnet-core the works

    //[System.Web.Http.Route("api/{controller}/{id}")]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class VideoStreamController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _environment;

        public VideoStreamController(AppSettings appSettings, IWebHostEnvironment environment)
        {
            (_appSettings, _environment) = (appSettings, environment);
        }

        [HttpGet("{name}")]
        public IActionResult GetVideoContent(string name)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), _appSettings.DefaultPathToVideo, name);//"TargetVideo.mp4"

            Stream video = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(video, "video/mp4", enableRangeProcessing: true);
        }




        //protected async void WriteContentToStream(Stream outputStream, HttpContent content, TransportContext transportContext)
        //{
        //    //path of file which we have to read//  
        //    //var filePath = HttpContext.Current.Server.MapPath("~/MicrosoftBizSparkWorksWithStartups.mp4");
        //    string filePath = Path.Combine(Directory.GetCurrentDirectory(), _appSettings.DefaultPathToVideo, "TargetVideo.mp4");
        //    //here set the size of buffer, you can set any size  
        //    int bufferSize = 1000;
        //    byte[] buffer = new byte[bufferSize];
        //    //here we re using FileStream to read file from server//  
        //    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //    {
        //        int totalSize = (int)fileStream.Length;
        //        /*here we are saying read bytes from file as long as total size of file 

        //        is greater then 0*/
        //        while (totalSize > 0)
        //        {
        //            int count = totalSize > bufferSize ? bufferSize : totalSize;
        //            //here we are reading the buffer from orginal file  
        //            int sizeOfReadedBuffer = fileStream.Read(buffer, 0, count);
        //            //here we are writing the readed buffer to output//  
        //            await outputStream.WriteAsync(buffer, 0, sizeOfReadedBuffer);
        //            //and finally after writing to output stream decrementing it to total size of file.  
        //            totalSize -= sizeOfReadedBuffer;
        //        }
        //    }
        //}

    }
}
