using BLL.Interfaces;
using DAL.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Traiting2.Extensions;

namespace Traiting2.BackgroundService
{
    public class UpdateRedisService: AuxiliaryLib.BaseBackgroundService.BackgroundService
    {
        private IAnnouncementService _announcementService;
        private IDistributedCache _cache;
        private readonly IServiceProvider _services;
        public UpdateRedisService(/*IAnnouncementService announcementService, IDistributedCache cache*/
            IServiceProvider Services)
        {
            //(_announcementService, _cache) =(announcementService, cache);
            _services = Services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            var scope = _services.CreateScope();
            _announcementService = scope.ServiceProvider.GetRequiredService<IAnnouncementService>();
            _cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
            //_announcementService = _services.GetService<IAnnouncementService>();
            //_cache = _services.GetService<IDistributedCache>();
            while (!stoppingToken.IsCancellationRequested)
            {
                //string recordKey = $"PopularAnnouncementData_{DateTime.Now.ToString("yyyyMMdd_hhmm")}";
                string recordKey = $"PopularAnnouncementData";

                List<Announcement> announcements = _announcementService.GetAll_Queryable().OrderByDescending(i => i.Views)
                        .Take(20).ToList();
                await _cache.SetRecordAsync<List<Announcement>>(recordKey, announcements);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
