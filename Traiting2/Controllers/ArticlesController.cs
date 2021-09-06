using AuxiliaryLib.Helpers;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.MongoEntity;
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
    public class ArticlesController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IMongoRepoArticle _mongoRepoArticle;

        public ArticlesController(AppSettings appSettings, IMongoRepoArticle mongoRepoArticle)
        {
            (_appSettings, _mongoRepoArticle) = (appSettings, mongoRepoArticle);
            

        }

        [HttpGet("GetArticles")]
        public IActionResult GetArticles()
        {
            PageResponse<string> pageResponse = new PageResponse<string>();
            return Ok(pageResponse);
        }

        [HttpPost("SaveArticle")]
        public IActionResult SaveArticle()
        {
            string content = "";
            using(StreamReader sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Temp",
                "News LIGA.net.html")))
            {
                content = sr.ReadToEnd();
            }
            Article article = new Article("News LIGA.net.html", content);
            _mongoRepoArticle.InsertAsync(article);
            return Ok();
        }
    }
}
