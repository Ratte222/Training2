using AuxiliaryLib.Helpers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BLL.Interfaces;
using BLL.Services;
using DAL.EF;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectForBenchmark.Benchmark
{
    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.ColdStart)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class IncludeBanchy
    {
        private AppDBContext _context;
        private IClientService _clientService;
        private readonly int _pageLength = 15;
        private readonly int _pageNumber = 5;
        private readonly string _clientId = "0004815d-ee49-478a-b3ee-f79ae94663b1", _userName = "anq2qO9tHC4UQJ36PC7wRpAd0",
            _email= "Pw8sKNC9BjrLEKfd4QsM9ILWw@gmail.com";

        [GlobalSetup]
        public void GlobalSetup()
        {
            //string connection = "Server=(localdb)\\mssqllocaldb;Database=Training2;Trusted_Connection=True;MultipleActiveResultSets=true";
            DbContextOptionsBuilder<AppDBContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
            dbContextOptionsBuilder.UseSqlServer(Program.connection);
            _context = new AppDBContext(dbContextOptionsBuilder.Options);
            _clientService = new ClientService(_context);
        }

        [Benchmark]
        public PageResponse<Announcement> GetClient_IncludeAnnouncement_ThenIncludeProductPhoto()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
            Client client = _context.Clients.Include(i => i.Announcements).ThenInclude(j => j.ProductPhotos)
                .AsNoTracking().FirstOrDefault(i => i.Id == _clientId);
            pageResponse.TotalItems = client.Announcements.Count;
            pageResponse.Items = client.Announcements.Skip(pageResponse.Skip).Take(pageResponse.Take).ToList();
            return pageResponse;
        }

        [Benchmark]
        public PageResponse<Announcement> GetAnnouncement_IncludePhoto()
        {
            PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);

            var query = _context.Announcements.Where(i => i.ClientId == _clientId).Include(j => j.ProductPhotos)
                .AsNoTracking();
            pageResponse.TotalItems = query.Count();
            pageResponse.Items = query.Skip(pageResponse.Skip).Take(pageResponse.Take).ToList();
            return pageResponse;
        }

        //[Benchmark]
        //public PageResponse<Announcement> GetAnnouncementWithPhoto_Join()
        //{
        //    //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/join-operations
        //    //https://entityframeworkcore.com/querying-data-joining
        //    PageResponse<Announcement> pageResponse = new PageResponse<Announcement>(_pageLength, _pageNumber);
        //    var query = _context.Announcements.Where(i => i.ClientId == _clientId)
        //        .Join(_context.ProductPhotos,
        //        ann => ann.Id,
        //        photo => photo.Announcement.Id,
        //        (ann, photo) => new Announcement()
        //        {
        //            Id = ann.Id,
        //            Name = ann.Name,
        //            Description = ann.Description,
        //            Cost = ann.Cost,
        //            ProductPhotos = new[] {
        //                new ProductPhoto()
        //                {
        //                    Id = photo.Id,
        //                    Name = photo.Name
        //                } }
        //        }).AsNoTracking();
        //    //var query = _context.Announcements.Where(i => i.ClientId == _clientId)
        //    //    .GroupJoin(_context.ProductPhotos,
        //    //    ann => ann.Id,
        //    //    photo => photo.AnnouncementId,
        //    //    (ann, photo) => ann).AsNoTracking();
        //    pageResponse.TotalItems = query.Count();
        //    //pageResponse.Items = query.Skip(pageResponse.Skip).Take(pageResponse.Take).ToList();
        //    return pageResponse;
        //}
    }

    //public class AnnouncementFast
    //{
    //    public long Id { get; set; }
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //    public long Cost { get; set; }
    //    public ICollection<ProductPhotoFast> ProductPhotos { get; set; }
    //    public IEnumerable<long> ProductPhotosId { get; set; }
    //}

    //public class ProductPhotoFast
    //{
    //    public long Id { get; set; }
    //    public string Name { get; set; }
    //}
}
