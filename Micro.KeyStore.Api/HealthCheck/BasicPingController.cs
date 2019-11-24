using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.KeyStore.Api.HealthCheck
{
    [ApiController]
    [Route("ping")]
    public class BasicPingController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PingResponse), StatusCodes.Status200OK)]
        public IActionResult Ping()
        {
            return Ok(new PingResponse
            {
                Ping = "Pong"
            });
        }
    }
}
