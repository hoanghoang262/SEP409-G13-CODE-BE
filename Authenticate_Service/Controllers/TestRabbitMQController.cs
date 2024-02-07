
using Account.API.Model;

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
       
       

        //private readonly ProductTestContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;


        public TestRabbitMQController( IPublishEndpoint publishEndpoint,IMapper mapper)
        {
            
            
            _publishEndpoint = publishEndpoint;
            _mapper=mapper;

        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(CourseMessage models)
        {

            var eventMessage = models;
           await _publishEndpoint.Publish(eventMessage);

            return Ok(eventMessage);
        }
        //[HttpGet]
        //public IActionResult ProductTest()
        //{
        //    var product = _context.ProductTests.ToList();


        //    return Ok(product);
        //}

    }
}
