using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorToDoList.Data;
using BlazorToDoList.Settings;

namespace BlazorToDoList.Services
{
    public class Authentication
    {
        private readonly ISupabaseConfig _supabaseConfig;

        public Authentication(ISupabaseConfig supabaseConfig)
        {
            _supabaseConfig = supabaseConfig;
        }
        
        public async Task<LoginContext> Register(string username, string password)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_supabaseConfig.GetUrl()}auth/v1/signup"),
                Headers =
                {
                    { "apikey", $"{_supabaseConfig.GetApiKey()}" },
                },
                Content = new StringContent($"{{\n  \"email\": \"{username}\",\n  \"password\": \"{password}\"\n}}")
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return await LoginAsync(username, password);
            }
        }
        public async Task<LoginContext> LoginAsync(string userName, string password)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_supabaseConfig.GetUrl()}auth/v1/token?grant_type=password"),
                    Headers =
                    {
                        {
                            "apikey",
                           await _supabaseConfig.GetApiKey()
                        },
                    },
                    Content = new StringContent(
                        $"{{\n  \"email\": \"{userName}\",\n  \"password\": \"{password}\"\n}}")
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };
                using var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var jsonObject = JsonSerializer.Deserialize<LoginContext>(body, options);
                return jsonObject;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}