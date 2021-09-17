using AutoMapper;
using AuxiliaryLib.Helpers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BLL.DTO.Announcement;
using DAL.EF;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using ProjectForBenchmark.AutoMapper;
using ProjectForBenchmark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectForBenchmark.Benchmark
{
    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.ColdStart, targetCount: 10)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class RedisBenchy
    {
        private AppDBContext _context;
        private IDistributedCache _cache;
        private IMapper _mapper;

        private readonly int _announcementsInRedisCache = 150;
        private readonly int _pageLength = 10;
        private readonly int _pageNumber = 5;

        [GlobalSetup]
        public void GlobalSetup()
        {
            //DbContextOptionsBuilder<AppDBContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
            //dbContextOptionsBuilder.UseSqlServer(Program.connection);
            //_context = new AppDBContext(dbContextOptionsBuilder.Options);
            var services = new ServiceCollection()
                .AddDbContext<DAL.EF.AppDBContext>(options =>
                options.UseSqlServer(Program.connection),
                ServiceLifetime.Transient)
                .AddAutoMapper(typeof(AutoMapperProfile)); 
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:5002";
                options.InstanceName = "Training2_";
            });
            var scope = services.BuildServiceProvider().CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            _cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
            _mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        }

        [Benchmark]
        public PageResponse<Announcement> GetMostPopularAnnouncementFromRedis()
        {
            string recordKey = $"PopularAnnouncementData";
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            List<Announcement> announcements = _cache.GetRecordAsync<List<Announcement>>(recordKey).GetAwaiter().GetResult();
            if (announcements is null)
            {
                return null;
            }
            pageResponse.TotalItems = _announcementsInRedisCache;
            pageResponse.Items = announcements.Skip(pageResponse.Skip).Take(pageResponse.Take).ToList();
            return pageResponse;
        }

        [Benchmark]
        public PageResponse<AnnouncementDTO> GetMostPopularAnnouncementFromRedis_UseMapper()
        {
            string recordKey = $"PopularAnnouncementData";
            PageResponse<AnnouncementDTO> pageResponse = new PageResponse<AnnouncementDTO>(_pageLength, _pageNumber);
            List<Announcement> announcements = _cache.GetRecordAsync<List<Announcement>>(recordKey).GetAwaiter().GetResult();
            if (announcements is null)
            {
                return null;
            }
            pageResponse.TotalItems = _announcementsInRedisCache;
            pageResponse.Items = _mapper.Map<List<Announcement>, List<AnnouncementDTO>>(announcements);
            return pageResponse;
        }

        [Benchmark]
        public PageResponse<Announcement> GetMostPopularAnnouncementFromDatabase()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            List<Announcement> announcements = _context.Announcements.AsNoTracking()
                    .OrderByDescending(i => i.Views).Skip(pageResponse.Skip).Take(pageResponse.Take)
                        .Include(i => i.ProductPhotos).ToList();
            if (announcements is null)
            {
                return null;
            }
            pageResponse.TotalItems = _announcementsInRedisCache;
            pageResponse.Items = announcements;
            return pageResponse;            
        }

        [Benchmark]
        public PageResponse<AnnouncementDTO> GetMostPopularAnnouncementFromDatabase_UseMapper()
        {
            PageResponse<AnnouncementDTO> pageResponse = new PageResponse<AnnouncementDTO>(_pageLength, _pageNumber);
            List<Announcement> announcements = _context.Announcements.AsNoTracking()
                    .OrderByDescending(i => i.Views).Skip(pageResponse.Skip).Take(pageResponse.Take)
                        .Include(i => i.ProductPhotos).ToList();
            if (announcements is null)
            {
                return null;
            }
            pageResponse.TotalItems = _announcementsInRedisCache;
            pageResponse.Items = _mapper.Map<List<Announcement>, List<AnnouncementDTO>>(announcements);
            return pageResponse;
        }
    }
}
