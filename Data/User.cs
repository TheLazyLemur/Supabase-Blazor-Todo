using System.Text.Json.Serialization;

namespace BlazorToDoList.Data
{
    public class LoginContext
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        public User User { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }
}