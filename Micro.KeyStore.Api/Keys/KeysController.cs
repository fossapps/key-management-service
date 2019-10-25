using System.Threading.Tasks;
using Micro.KeyStore.Api.Keys.Models;
using Micro.KeyStore.Api.Keys.Repositories;
using Micro.KeyStore.Api.Keys.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.KeyStore.Api.Keys
{
    [ApiController]
    [Route("api/keys")]
    public class KeysController : ControllerBase
    {
        private readonly IKeyService _keyService;
        private readonly IKeyRepository _keyRepository;

        public KeysController(IKeyService keyService, IKeyRepository keyRepository)
        {
            _keyService = keyService;
            _keyRepository = keyRepository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(KeyCreatedResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(CreateKeyRequest request)
        {
            var key = new Key
            {
                Body = request.Body,
                Sha = Sha256.Compute(request.Body),
            };
            var result = await _keyService.CreateKey(key);
            var keyCreatedResponse = new KeyCreatedResponse
            {
                Body = result.Body,
                Id = result.ShortSha,
            };
            return CreatedAtAction("Get", "Keys", new {id = keyCreatedResponse.Id}, keyCreatedResponse);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(KeyCreatedResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id)
        {
            var key = await _keyRepository.FindByShortSha(id);
            var res = new KeyCreatedResponse
            {
                Body = key.Body,
                Id = key.ShortSha,
            };
            return Ok(res);
        }
    }
}
