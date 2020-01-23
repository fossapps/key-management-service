using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.KeyStore.Api.HealthCheck
{
    [ApiController]
    [Route("")]
    public class BasicPingController : ControllerBase
    {
        [HttpGet("ping")]
        [ProducesResponseType(typeof(PingResponse), StatusCodes.Status200OK)]
        public IActionResult Ping()
        {
            return Ok(new PingResponse
            {
                Ping = "Pong"
            });
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectPermanent("/health");
        }
    }
}
