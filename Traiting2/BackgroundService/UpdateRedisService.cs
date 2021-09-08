using BLL.Interfaces;
using DAL.Model;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IAnnouncementService _announcementService;
        private readonly IDistributedCache _cache;
        public UpdateRedisService(IAnnouncementService announcementService, IDistributedCache cache)
        {
            (_announcementService, _cache) =(announcementService, cache);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
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
