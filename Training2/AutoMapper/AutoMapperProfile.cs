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
using Training2.Model;

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
            CreateMap<Log, MyLoggerUiModel>()
                .ForMember(dest => dest.LogLevel, opt => opt.MapFrom(scr => scr.Level))
                .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(scr => scr.TimeStamp.ToString()));
        }
    }
}
