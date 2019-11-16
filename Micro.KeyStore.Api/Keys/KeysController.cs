using System;
using System.Threading.Tasks;
using Micro.KeyStore.Api.Keys.Models;
using Micro.KeyStore.Api.Keys.Repositories;
using Micro.KeyStore.Api.Keys.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Micro.KeyStore.Api.Keys
{
    [ApiController]
    [Route("api/keys")]
    public class KeysController : ControllerBase
    {
        private readonly IKeyService _keyService;
        private readonly IKeyRepository _keyRepository;
        private readonly ILogger<KeysController> _logger;

        public KeysController(IKeyService keyService, IKeyRepository keyRepository, ILogger<KeysController> logger)
        {
            _keyService = keyService;
            _keyRepository = keyRepository;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(KeyCreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(CreateKeyRequest request)
        {
            var key = new Key
            {
                Body = request.Body,
                Sha = Sha256.Compute(request.Body)
            };
            try
            {
                var result = await _keyService.CreateKey(key);
                var keyCreatedResponse = new KeyCreatedResponse
                {
                    Body = result.Body,
                    Id = result.ShortSha
                };
                return CreatedAtAction("Get", "Keys", new {id = keyCreatedResponse.Id}, keyCreatedResponse);
            }
            catch (ConflictingKeyConflictException e)
            {
                _logger.LogError(e, "caught conflicting exception");
                ModelState.AddModelError("Errors", "Key already exists");
                return Conflict(new ValidationProblemDetails(ModelState));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Errors", "Unknown error occurred");
                _logger.LogError(e, "caught exception");
                return new ObjectResult(new ValidationProblemDetails(ModelState)) {StatusCode = StatusCodes.Status500InternalServerError};
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(KeyCreatedResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var key = await _keyRepository.FindByShortSha(id);
            if (key == null)
            {
                ModelState.AddModelError("Errors", "Not found");
                return NotFound(new ValidationProblemDetails(ModelState));
            }
            var res = new KeyCreatedResponse
            {
                Body = key.Body,
                Id = key.ShortSha
            };
            return Ok(res);
        }
    }
}
