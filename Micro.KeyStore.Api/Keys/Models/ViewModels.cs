using System.ComponentModel.DataAnnotations;

namespace Micro.KeyStore.Api.Keys.Models
{
    public class CreateKeyRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Body is required")]
        public string Body { set; get; }
    }

    public class KeyCreatedResponse
    {
        public string Body { set; get; }
        public string Id { set; get; }
    }
}
