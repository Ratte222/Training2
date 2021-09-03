﻿using AutoMapper;
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
using Traiting2.Extensions;

namespace Traiting2.Controllers
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
    }
}
