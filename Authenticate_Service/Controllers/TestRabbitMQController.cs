
using Account.API.Model;
using Authenticated.Models;
using AutoMapper;
using EventBus.Message.IntegrationEvent.Event;
using Infrastructures;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authenticated.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestRabbitMQController : ControllerBase
    {
       
       

        private readonly ProductTestContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;


        public TestRabbitMQController(ProductTestContext context, IPublishEndpoint publishEndpoint,IMapper mapper)
        {
            
            _context = context;
            _publishEndpoint = publishEndpoint;
            _mapper=mapper;

        }


        [HttpPost]
        public IActionResult SendMessage(LoginEvent models)
        {

            var eventMessage = models;
            _publishEndpoint.Publish(eventMessage);

            return Ok();
        }
        [HttpGet]
        public IActionResult ProductTest()
        {
            var product = _context.ProductTests.ToList();


            return Ok(product);
        }

    }
}
