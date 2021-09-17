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
    public class Program
    {
        public const string connection = "Server=(localdb)\\mssqllocaldb;Database=Training2;Trusted_Connection=True;MultipleActiveResultSets=true";
        static void Main(string[] args)
        {
            ServiceProvider services = null;
            Startup startup = new Startup();
            startup.ConfigureService(ref services);
            var scope = services.CreateScope();
            //Test(scope);

            //BenchmarkRunner.Run<TestBency>();
            BenchmarkRunner.Run<AnnouncementBenchy>();
            //BenchmarkRunner.Run<EntitySearchBenchy>();
            //BenchmarkRunner.Run<IncludeBanchy>();
            //BenchmarkRunner.Run<RedisBenchy>();
            Console.WriteLine("Hello World!");
        }


        private static void Test(IServiceScope scope)
        {
            var _context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            SQL_Where_Count_OrderBy_Skip_Take_ToList(_context);
            #region JoinTable
            var query = _context.Announcements.Where(i => i.ClientId == "0004815d-ee49-478a-b3ee-f79ae94663b1")
                .Join(_context.ProductPhotos,
                ann => ann.Id,
                photo => photo.Announcement.Id,
                (ann, photo) => new Announcement()
                {
                    Id = ann.Id,
                    Name = ann.Name,
                    Description = ann.Description,
                    Cost = ann.Cost,
                    ProductPhotos = new[] {
                        new ProductPhoto()
                        {
                            Id = photo.Id,
                            Name = photo.Name,
                            AnnouncementId = ann.Id
                        } }
                }).AsNoTracking().ToList();
            //var query_ = query.GroupBy().ToList();
            var query_ = _context.Announcements.Where(i => i.ClientId == "0004815d-ee49-478a-b3ee-f79ae94663b1")
                .Include(j => j.ProductPhotos)
                .AsNoTracking().ToList();
            query = query.Distinct().ToList();
            #endregion
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
                $" WHERE {nameof(temp.Category)} = {(int)Category.auto} ORDER BY {nameof(temp.Cost)} ASC " +
                    $"LIMIT {pageResponse.Skip}, {pageResponse.Take}").ToList();
            //pageResponse.Items = _context.Announcements.FromSqlRaw($"SELECT * FROM {nameof(Announcement)}s" +
            //    $" WHERE {nameof(temp.Category)} = {(int)Category.auto} ORDER BY {nameof(temp.Cost)} asc " +
            //        $"OFFSET {pageResponse.Skip} ROWS FETCH NEXT {pageResponse.Take} ROWS ONLY").ToList();
            #endregion
        }

        public static PageResponse<Announcement> SQL_Where_Count_OrderBy_Skip_Take_ToList(AppDBContext _context)
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(100, 1003);
            Announcement temp = new Announcement();
            pageResponse.TotalItems = _context.Announcements.FromSqlRaw($"SELECT * FROM {nameof(Announcement)}s" +
                $" WHERE {nameof(temp.Category)} = {(int)Category.auto}").Count();
            pageResponse.Items = _context.Announcements.FromSqlRaw($"SELECT * FROM {nameof(Announcement)}s" +
                $" WHERE {nameof(temp.Category)} = {(int)Category.auto} ORDER BY {nameof(temp.Cost)} ASC " +
                    $"LIMIT {pageResponse.Skip}, {pageResponse.Take}").ToList();
            return pageResponse;
        }
    }
}
