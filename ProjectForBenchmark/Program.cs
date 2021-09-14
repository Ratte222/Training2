using BLL.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using BenchmarkDotNet.Running;
using System.Linq;
using DAL.EF;
using AuxiliaryLib.Helpers;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ProjectForBenchmark.Benchmark;

namespace ProjectForBenchmark
{
    //https://benchmarkdotnet.org/articles/samples/IntroConfigSource.html
    class Program
    {
        static void Main(string[] args)
        {
            ServiceProvider services = null;
            Startup startup = new Startup();
            startup.ConfigureService(ref services);
            var scope = services.CreateScope();
            //Test(scope);
            
            //BenchmarkRunner.Run<TestBency>();
            //BenchmarkRunner.Run<AnnouncementBenchy>();
            BenchmarkRunner.Run<EntitySearchBenchy>();
            Console.WriteLine("Hello World!");
        }

        private static void Test(IServiceScope scope)
        {
            var _context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            #region FindProductPhoto
            //Predicate<ProductPhoto> predicate = i => i.Id == 4360901;
            //var productPhoto =  _context.ProductPhotos.Find(predicate);
            #endregion
            #region SQL_Select_Annoncement
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(15, 10);
            Announcement temp = new Announcement();

            //var temp_ = _context.Announcements.FromSqlRaw($"SELECT " +
            //    $"COUNT(*) FROM {nameof(Announcement)}s " +
            //    $"WHERE {nameof(temp.Category)} = {(int)Category.auto}").ToList();
            var announcements = _context.Announcements.Where(i => i.Category == Category.auto);
            pageResponse.TotalItems = announcements.Count();
            pageResponse.TotalItems = _context.Announcements.FromSqlRaw($"SELECT * FROM {nameof(Announcement)}s" +
                $" WHERE {nameof(temp.Category)} = {(int)Category.auto}").Count();
            pageResponse.Items = _context.Announcements.FromSqlRaw($"SELECT * FROM {nameof(Announcement)}s" +
                $" WHERE {nameof(temp.Category)} = {(int)Category.auto} ORDER BY {nameof(temp.Cost)} asc " +
                    $"OFFSET {pageResponse.Skip} ROWS FETCH NEXT {pageResponse.Take} ROWS ONLY").ToList();
            #endregion
        }
    }
}
