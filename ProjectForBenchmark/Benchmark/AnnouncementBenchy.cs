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
using BenchmarkDotNet.Engines;
using System.Threading.Tasks;
using System.Linq;

namespace ProjectForBenchmark.Benchmark
{
    //https://coderoad.ru/58972840/%D0%9A%D0%B0%D0%BA-%D0%BC%D1%8B-%D0%BC%D0%BE%D0%B6%D0%B5%D0%BC-%D0%BF%D0%B5%D1%80%D0%B5%D0%B4%D0%B0%D1%82%D1%8C-%D0%B4%D0%B8%D0%BD%D0%B0%D0%BC%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B8%D0%B5-%D0%B0%D1%80%D0%B3%D1%83%D0%BC%D0%B5%D0%BD%D1%82%D1%8B-%D0%B2-%D1%82%D0%B5%D0%B3%D0%B5-Arguments-%D0%B4%D0%BB%D1%8F
    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.ColdStart, targetCount: 10)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class AnnouncementBenchy
    {
        private IAnnouncementService _announcementService;
        private AppDBContext _context;

        private readonly int _pageLength = 100;
        private readonly int _pageNumber = 10003;

        //private readonly IMapper _mapper;
        //public AnnouncementBenchy(IAnnouncementService announcementService, IMapper mapper)
        //{
        //    (_announcementService, _mapper)
        //        = (announcementService, mapper);
        //}

        [GlobalSetup]
        public void GlobalSetup()
        {
            DbContextOptionsBuilder<AppDBContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
            dbContextOptionsBuilder.UseSqlServer(Program.connection);
            _context =  new AppDBContext(dbContextOptionsBuilder.Options);
            _announcementService = new AnnouncementService(_context);
        }

        [Benchmark]
        public PageResponse<Announcement> GetAllQuerable_ToList_Count_Skip_Take_ToList()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            List<Announcement> announcements = _announcementService.GetAll_Queryable().ToList();
            pageResponse.TotalItems = announcements.Count();
            pageResponse.Items = announcements.Skip(pageResponse.Skip).Take(pageResponse.Take).ToList();
            return pageResponse;
        }

        [Benchmark]
        public PageResponse<Announcement> GetAllQuerable_Count_Skip_Take_ToList()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            var announcements = _announcementService.GetAll_Queryable();
            pageResponse.TotalItems = announcements.Count();
            pageResponse.Items = announcements.Skip(pageResponse.Skip).Take(pageResponse.Take).ToList();
            return pageResponse;
        }

        [Benchmark]
        public PageResponse<Announcement> GetAllEnumerable_Count_Skip_Take_ToList()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            var announcements = _announcementService.GetAll_Enumerable();
            pageResponse.TotalItems = announcements.Count();
            pageResponse.Items = announcements.Skip(pageResponse.Skip).Take(pageResponse.Take).ToList();
            return pageResponse;
        }

        [Benchmark]
        public PageResponse<Announcement> SQL_Count_Skip_Take_ToList()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            Announcement temp = new Announcement();
            pageResponse.TotalItems = _context.Announcements.Count();
            pageResponse.Items = _context.Announcements.FromSqlRaw($"SELECT * FROM {nameof(Announcement)}s" +
                $" WHERE {nameof(temp.Category)} = {(int)Category.auto} ORDER BY {nameof(temp.Cost)} asc " +
                    $"OFFSET {pageResponse.Skip} ROWS FETCH NEXT {pageResponse.Take} ROWS ONLY").ToList();
            return pageResponse;
        }

        [Benchmark]
        public async Task<PageResponse<Announcement>> GetAllQuerable_Count_Skip_Take_ToList__Async()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            var announcements = _announcementService.GetAll_Queryable();
            pageResponse.TotalItems = await announcements.CountAsync();
            pageResponse.Items = await announcements.Skip(pageResponse.Skip).Take(pageResponse.Take).ToListAsync();
            return pageResponse;
        }

        [Benchmark]
        public async Task<PageResponse<Announcement>> DbAnnouncements_Count_Skip_Take_ToList__Async()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            var announcements = _context.Announcements;
            pageResponse.TotalItems = await announcements.CountAsync();
            pageResponse.Items = await announcements.Skip(pageResponse.Skip).Take(pageResponse.Take).ToListAsync();
            return pageResponse;
        }

        [Benchmark]
        public async Task<PageResponse<Announcement>> DbAnnouncements_AsNoTraking_Count_Skip_Take_ToList__Async()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            var announcements = _context.Announcements.AsNoTracking();
            pageResponse.TotalItems = await announcements.CountAsync();
            pageResponse.Items = await announcements.Skip(pageResponse.Skip).Take(pageResponse.Take).ToListAsync();
            return pageResponse;
        }

        [Benchmark]
        public PageResponse<Announcement> GetAllQuerable_Where_Count_OrderBy_Skip_Take_ToList()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            var announcements = _announcementService.GetAll_Queryable().Where(i => i.Category == Category.auto);
            pageResponse.TotalItems = announcements.Count();
            pageResponse.Items = announcements.OrderBy(i => i.Cost)
                .Skip(pageResponse.Skip).Take(pageResponse.Take).ToList();
            return pageResponse;
        }

        [Benchmark]
        public PageResponse<Announcement> GetAllEnumerable_Where_Count_OrderBy_Skip_Take_ToList()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            var announcements = _announcementService.GetAll_Enumerable().Where(i => i.Category == Category.auto);
            pageResponse.TotalItems = announcements.Count();
            pageResponse.Items = announcements.OrderBy(i => i.Cost)
                .Skip(pageResponse.Skip).Take(pageResponse.Take).ToList();
            return pageResponse;
        }

        [Benchmark]
        public PageResponse<Announcement> SQL_Where_Count_OrderBy_Skip_Take_ToList()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            Announcement temp = new Announcement();
            //pageResponse.TotalItems = _context.Announcements.Count();
            pageResponse.TotalItems = _context.Announcements.FromSqlRaw($"SELECT * FROM {nameof(Announcement)}s" +
                $" WHERE {nameof(temp.Category)} = {(int)Category.auto}").Count();
            pageResponse.Items = _context.Announcements.FromSqlRaw($"SELECT * FROM {nameof(Announcement)}s" +
                $" WHERE {nameof(temp.Category)} = {(int)Category.auto} ORDER BY {nameof(temp.Cost)} asc " +
                    $"OFFSET {pageResponse.Skip} ROWS FETCH NEXT {pageResponse.Take} ROWS ONLY").ToList();
            return pageResponse;
        }
    }
}
