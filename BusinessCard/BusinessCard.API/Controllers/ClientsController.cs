using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private ILogger<ClientsController> _logger;

        private readonly IMediator _mediator;

        public ClientsController(IMediator mediator, ILogger<ClientsController> logger)
        {
                
        }
        
    }
}
