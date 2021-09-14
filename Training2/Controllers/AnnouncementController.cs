using AutoMapper;
using AuxiliaryLib.Helpers;
using BLL.DTO.Announcement;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Training2.Extensions;

namespace Training2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IAnnouncementService _announcementService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public AnnouncementController(AppSettings appSettings, IAnnouncementService announcementService, 
            IMapper mapper, IDistributedCache cache)
        {
            (_appSettings, _announcementService, _mapper, _cache)=(appSettings, announcementService, mapper, cache);
        }

        [HttpPost("AddAnnouncement")]
        public IActionResult AddAnnouncement(NewAnnouncementDTO newAnnouncementDTO)
        {
            Announcement announcement = _mapper.Map<NewAnnouncementDTO, Announcement>(newAnnouncementDTO);
            _announcementService.Create(announcement);
            return Ok("Announcement creeated successfully");
        }

        [HttpGet("GetMostPopularAnnouncement")]
        public async Task<IActionResult> GetMostPopularAnnouncement(int? pageLength = null,
            int? pageNumber = null)
        {
            string recordKey = $"PopularAnnouncementData";
            PageResponse<AnnouncementDTO> pageResponse = new PageResponse<AnnouncementDTO>(pageLength, pageNumber);
            List<Announcement> announcements = await _cache.GetRecordAsync<List<Announcement>>(recordKey);
            if (announcements is null)
            {
                announcements = _announcementService.GetAll_Queryable().OrderByDescending(i => i.Views)
                    .Take(pageResponse.Take).ToList();
                //await _cache.SetRecordAsync<List<Announcement>>(recordKey, announcements);
            }
            pageResponse.TotalItems = announcements.Count();
            pageResponse.Items = _mapper.Map<IEnumerable<Announcement>, List<AnnouncementDTO>>(
                announcements.Skip(pageResponse.Skip).Take(pageResponse.Take));
            return Ok(pageResponse);
        }

        [HttpGet("GetAnnouncement")]
        public IActionResult GetAnnouncement(long id)
        {
            Announcement announcement = _announcementService.Get(i => i.Id == id);
            AnnouncementDTO announcementDTO = _mapper.Map<Announcement, AnnouncementDTO>(
                announcement);
            //var options = new JsonSerializerOptions()
            //{
            //    MaxDepth = 0,
            //    IgnoreNullValues = true,
            //    IgnoreReadOnlyProperties = true
            //};
            
            return Ok(/*JsonSerializer.Serialize(announcementDTO, options)*/announcementDTO);
        }

        [HttpGet("GetAnnouncements")]
        public async Task<IActionResult> GetAnnouncements(int? pageLength = null,
            int? pageNumber = null)
        {
            //PageResponse<AnnouncementDTO> pageResponse = new PageResponse<AnnouncementDTO>(pageLength, pageNumber);
            //List<Announcement> announcements = _announcementService.GetAll_Queryable().ToList();
            //pageResponse.TotalItems = announcements.Count();
            //pageResponse.Items = _mapper.Map<IEnumerable<Announcement>, List<AnnouncementDTO>>(
            //    announcements.Skip(pageResponse.Skip).Take(pageResponse.Take));
            PageResponse<AnnouncementDTO> pageResponse = new PageResponse<AnnouncementDTO>(pageLength, pageNumber);
            var announcements = _announcementService.GetAll_Queryable();
            pageResponse.TotalItems = await announcements.CountAsync();
            pageResponse.Items = _mapper.Map<List<Announcement>, List<AnnouncementDTO>>(
                await announcements.Skip(pageResponse.Skip).Take(pageResponse.Take).ToListAsync());
            return Ok(pageResponse);
        }
    }
}
