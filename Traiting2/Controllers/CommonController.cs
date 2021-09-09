using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Traiting2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        [HttpGet("GetLogo")]
        public IActionResult GetLogo()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "logo");
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Ok(fileStream);
            //string content = "";
            //using (StreamReader sr = new StreamReader(path))
            //{
            //    content = sr.ReadToEnd();
            //}
            //return Content(content);
        }
    }
}
