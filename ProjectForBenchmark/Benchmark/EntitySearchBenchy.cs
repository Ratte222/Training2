using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BLL.Interfaces;
using BLL.Services;
using DAL.EF;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectForBenchmark.Benchmark
{
    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.ColdStart, targetCount: 10)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class EntitySearchBenchy
    {
        private AppDBContext _context;
        private readonly long _productPhotoId = 4360901;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var services = new ServiceCollection()
                .AddDbContext<DAL.EF.AppDBContext>(options =>
                options.UseMySql(Program.connection, new MySqlServerVersion(new Version(8, 0, 26))),
                ServiceLifetime.Transient);
            var scope = services.BuildServiceProvider().CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

        }

        [Benchmark]
        public ProductPhoto Get_Find()
        {
            Predicate<ProductPhoto> predicate = i => i.Id == _productPhotoId;
            return _context.ProductPhotos.Find(_productPhotoId);
        }

        [Benchmark]
        public ProductPhoto SQL_Select_Where_FirstOrDefault()
        {
            return _context.ProductPhotos.FromSqlRaw($"SELECT * FROM {nameof(ProductPhoto)}s" +
                $" WHERE Id = {_productPhotoId}").FirstOrDefault();
        }

        [Benchmark]
        public async Task<ProductPhoto> Get_FindAsync()
        {
            return await _context.ProductPhotos.FindAsync(_productPhotoId);
        }

        [Benchmark]
        public ProductPhoto Get_First()
        {
            Func<ProductPhoto, bool> func = i => i.Id == _productPhotoId;
            return _context.ProductPhotos.First(func);
        }

        [Benchmark]
        public ProductPhoto Get_FirstOrDefault()
        {
            Func<ProductPhoto, bool> func = i => i.Id == _productPhotoId;
            return _context.ProductPhotos.FirstOrDefault(func);
        }

        [Benchmark]
        public async Task<ProductPhoto> Get_FirstOrDefaultAsync()
        {
            Expression<Func<ProductPhoto, bool>> func = i => i.Id == _productPhotoId;
            return await _context.ProductPhotos.FirstOrDefaultAsync(func);
        }

        [Benchmark]
        public ProductPhoto Get_Single()
        {
            Func<ProductPhoto, bool> func = i => i.Id == _productPhotoId;
            return _context.ProductPhotos.Single(func);
        }
    }
}
