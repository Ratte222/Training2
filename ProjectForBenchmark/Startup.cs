using BLL.Interfaces;
using BLL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectForBenchmark.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectForBenchmark
{
    public class Startup
    {
        public void ConfigureService(ref ServiceProvider serviceCollection)
        {
            //setup our DI
            var services = new ServiceCollection()
                .AddAutoMapper(typeof(AutoMapperProfile));
            services.AddDbContext<DAL.EF.AppDBContext>(options => 
                options.UseMySql(Program.connection, new MySqlServerVersion(new Version(8, 0, 26))),
                ServiceLifetime.Transient);
            services.AddScoped<IAnnouncementService, AnnouncementService>();
            serviceCollection = services.BuildServiceProvider();
        }
    }
}
