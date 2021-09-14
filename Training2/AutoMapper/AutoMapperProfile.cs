using AutoMapper;
using BLL.DTO.Announcement;
using BLL.DTO.Article;
using BLL.DTO.Client;
using BLL.DTO.ProductPhoto;
using DAL.Model;
using DAL.MongoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training2.AutoMapper
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
