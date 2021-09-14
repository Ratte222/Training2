using System;
using System.Collections.Generic;
using System.Text;
using AuxiliaryLib.Helpers;
using BenchmarkDotNet.Attributes;
using BLL.DTO.Announcement;
using BLL.Interfaces;
using DAL.Model;
using System.Linq;
using AutoMapper;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using BLL.Services;

namespace ProjectForBenchmark.Benchmark
{
    //https://coderoad.ru/58972840/%D0%9A%D0%B0%D0%BA-%D0%BC%D1%8B-%D0%BC%D0%BE%D0%B6%D0%B5%D0%BC-%D0%BF%D0%B5%D1%80%D0%B5%D0%B4%D0%B0%D1%82%D1%8C-%D0%B4%D0%B8%D0%BD%D0%B0%D0%BC%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B8%D0%B5-%D0%B0%D1%80%D0%B3%D1%83%D0%BC%D0%B5%D0%BD%D1%82%D1%8B-%D0%B2-%D1%82%D0%B5%D0%B3%D0%B5-Arguments-%D0%B4%D0%BB%D1%8F
    [MemoryDiagnoser]
    public class AnnouncementBenchy
    {
        private IAnnouncementService _announcementService;
        //private readonly IMapper _mapper;
        //public AnnouncementBenchy(IAnnouncementService announcementService, IMapper mapper)
        //{
        //    (_announcementService, _mapper)
        //        = (announcementService, mapper);
        //}

        [GlobalSetup]
        public void GlobalSetup()
        {
            string connection = "Server=(localdb)\\mssqllocaldb;Database=Training2;Trusted_Connection=True;MultipleActiveResultSets=true";
            DbContextOptionsBuilder<AppDBContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
            dbContextOptionsBuilder.UseSqlServer(connection);
            var db =  new AppDBContext(dbContextOptionsBuilder.Options);
            _announcementService = new AnnouncementService(db);
        }

        [Benchmark]
        public PageResponse<Announcement> GetClientAnnouncements()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(15, 10);
            List<Announcement> announcements = _announcementService.GetAll_Queryable().ToList();
            pageResponse.TotalItems = announcements.Count();
            pageResponse.Items = announcements.Skip(pageResponse.Skip).Take(pageResponse.Take).ToList();
            return pageResponse;
        }
    }
}
