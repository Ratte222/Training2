using AutoMapper;
using BLL.DTO.Client;
using BLL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Traiting2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        public ClientController(IMapper mapper, IClientService clientService)
        {
            _mapper = mapper;
            _clientService = clientService;
        }

        
        [HttpGet("GetClientInfo")]
        [Authorize]
        public IActionResult GetClientInfo(string clientId = null)
        {
            if (clientId == null)
            {
                clientId = User.Identity.Name;
            }
            ClientDTO client = _mapper.Map<Client, ClientDTO>(_clientService.Get(i => i.Id == clientId));
            return Ok(client);
        }
    }
}
