
using Account.API.Model;
using Authenticated.Models;
using Infrastructures;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authenticated.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestRabbitMQController : ControllerBase
    { private readonly List<LoginModels> loginModels = new();
     private readonly IMessageProducer _messageProducer;
     private readonly ProductTestContext _context;
        private readonly IPublishEndpoint _publishEndpoint;


        public TestRabbitMQController(ProductTestContext context, IMessageProducer messageProducer,IPublishEndpoint publishEndpoint)
        {
            _messageProducer = messageProducer;
            _context = context;
            _publishEndpoint = publishEndpoint;

        }


        [HttpPost]
        public IActionResult SendMessage(LoginModels models)
        {

            loginModels.Add(models);
            _publishEndpoint.Publish(models);

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
