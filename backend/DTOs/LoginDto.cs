using Newtonsoft.Json;

namespace backend.DTOs
{
    public class LoginDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}