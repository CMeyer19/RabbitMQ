using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Producer.RabbitMQ;

namespace Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValueController : ControllerBase
    {
        private readonly ILogger<ValueController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public ValueController(
            ILogger<ValueController> logger,
            IPublishEndpoint publishEndpoint
        )
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value)
        {
            await _publishEndpoint.Publish<ValueEntered>(new { Value = value });

            return Ok();
        }
    }
}
