using BLL.DTO.Client;
using DAL.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.MongoEntity;
using BLL.DTO.Article;
using BLL.DTO.Announcement;
using BLL.DTO.ProductPhoto;

namespace ProjectForBenchmark.AutoMapper
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
            CreateMap<NewAnnouncementDTO, Announcement>();
            CreateMap<Announcement, AnnouncementDTO>();
            CreateMap<ProductPhoto, ProductPhotoDTO>();
        }
    }
}
