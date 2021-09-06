using AutoMapper;
using BLL.DTO.Article;
using BLL.DTO.Client;
using DAL.Model;
using DAL.MongoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Traiting2.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Client, ClientDTO>();
            CreateMap<ClientDTO, Client>();
            CreateMap<Article, ArticleDTO>();
            CreateMap<ArticleDTO, Article>();
            CreateMap<NewArticle, Article>();
        }
    }
}
