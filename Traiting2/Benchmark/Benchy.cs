using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AuxiliaryLib.Helpers;
using BenchmarkDotNet.Attributes;
using BLL.DTO.Announcement;
using BLL.Interfaces;
using DAL.Model;

namespace Traiting2.Benchmark
{
    [MemoryDiagnoser]
    public class Benchy
    {
        private readonly IClientService _clientService;
        private readonly IProductPhotoService _productPhotoService;
        private readonly IAnnouncementService _announcementService;
        private readonly IMapper _mapper;
        public Benchy(IClientService clientService, IProductPhotoService productPhotoService,
            IAnnouncementService announcementService, IMapper mapper)
        {
            (_clientService, _productPhotoService, _announcementService, _mapper) 
                = (clientService, productPhotoService, announcementService, mapper);
        }

        [Benchmark]
        public PageResponse<AnnouncementDTO> GetClientAnnouncements()
        {
            PageResponse<AnnouncementDTO> pageResponse = new PageResponse<AnnouncementDTO>(15, 10);
            List<Announcement> announcements = _announcementService.GetAll_Queryable().ToList();
            pageResponse.TotalItems = announcements.Count();
            pageResponse.Items = _mapper.Map<IEnumerable<Announcement>, List<AnnouncementDTO>>(
                announcements.Skip(pageResponse.Skip).Take(pageResponse.Take));
            return pageResponse;
        }
    }
}
