using AutoMapper;
using AuxiliaryLib.Helpers;
using BLL.DTO.Article;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.MongoEntity;
using Microsoft.AspNetCore.Authorization;
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
        public readonly IMapper _mapper;

        public ArticlesController(AppSettings appSettings, IMongoRepoArticle mongoRepoArticle,
            IMapper mapper)
        {
            (_appSettings, _mongoRepoArticle, _mapper) 
                = (appSettings, mongoRepoArticle, mapper);
            

        }

        [Authorize]
        [HttpGet("GetArticles")]
        public IActionResult GetArticles(int? pageLength = null,
            int? pageNumber = null)
        {
            PageResponse<ArticleDTO> pageResponse = new PageResponse<ArticleDTO>(pageLength, pageNumber);
            List<Article> articles = _mongoRepoArticle.Get();
            pageResponse.TotalItems = articles.Count;
            pageResponse.Items = _mapper.Map<IEnumerable<Article>, List<ArticleDTO>>(
                articles.Skip(pageResponse.Skip).Take(pageResponse.Take));
            return Ok(pageResponse);
        }
        [HttpGet("GetArticle")]
        public IActionResult GetArticle(string articleId)
        {
            Article article = _mongoRepoArticle.Get(articleId);
            if (article is null)
                return NotFound();
            ArticleDTO articleDTO = _mapper.Map<Article, ArticleDTO>(article);
            return Ok(articleDTO);
        }
        //6135faf1a366be4bb6c53240
        [HttpPost("CreateArticle")]
        public IActionResult CreateArticle(NewArticle newArticle)
        {
            //string content = "";
            //using(StreamReader sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Temp",
            //    "News LIGA.net.html")))
            //{
            //    content = sr.ReadToEnd();
            //}
            Article article = _mapper.Map<NewArticle, Article>(newArticle);
            _mongoRepoArticle.Create(article);
            return Ok($"Article created successfully. Id = {article.Id}");
        }

        [HttpPut("UpdateArticle")]
        public IActionResult UpdateArticle(ArticleDTO articleDTO)
        {
            _mongoRepoArticle.Update(_mapper.Map<ArticleDTO, Article>(articleDTO));
            return Ok("Article updated successfully");
        }
        [HttpPut("RemoveArticle")]
        public IActionResult RemoveArticle(string articleId)
        {
            if(String.IsNullOrEmpty(articleId))
            {
                return BadRequest($"Fill {nameof(articleId)}");
            }
            _mongoRepoArticle.Remove(articleId);
            return Ok("Article updated successfully");
        }
    }
}
