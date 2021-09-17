using AutoMapper;
using AuxiliaryLib.Helpers;
using BLL.DTO.Client;
using BLL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Training2.Extensions;

namespace Training2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        private readonly IDistributedCache _cache;
        public ClientController(IMapper mapper, IClientService clientService, IDistributedCache cache)
        {
            _mapper = mapper;
            _clientService = clientService;
            _cache = cache;
        }


        //[Authorize]
        [HttpGet("GetClientInfo")]        
        public async Task<IActionResult> GetClientInfo(string clientId = null)
        {
            if (clientId == null)
            {
                clientId = User.Identity.Name;
            }
            string recordKey = $"ClientData_{DateTime.Now.ToString("yyyyMMdd_hhmm")}";

            Client client = await _cache.GetRecordAsync<Client>(recordKey);
            if(client is null)
            {
                client = _clientService.Get(i =>
                i.Id.ToLower() == clientId.ToLower());
                await _cache.SetRecordAsync<Client>(recordKey, client);
            }
            if(client is null)
            {
                return NotFound();
            }            
            return Ok(_mapper.Map<Client, ClientDTO>(client));
        }

        [HttpGet("GetClients")]
        public IActionResult GetClients(int? pageLength = null,
            int? pageNumber = null)
        {
            PageResponse<ClientDTO> pageResponse = new PageResponse<ClientDTO>(pageLength, pageNumber);
            var clients = _clientService.GetAll_Queryable();
            pageResponse.TotalItems = clients.Count();
            pageResponse.Items = _mapper.Map<IEnumerable<Client>, List<ClientDTO>>(
                clients.Skip(pageResponse.Skip).Take(pageResponse.Take));
            return Ok(pageResponse);
        }
    }
}
