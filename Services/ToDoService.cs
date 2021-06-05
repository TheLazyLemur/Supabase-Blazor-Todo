using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorToDoList.Data;
using BlazorToDoList.Settings;
using Microsoft.AspNetCore.Components;

namespace BlazorToDoList.Services
{
    public interface IToDoService
    {
        Task<List<ToDoItem>> GetAllToDosAsync();
        Task InsertToDoAsync(ToDoItem toDoItem);
        Task MarkAsCompleteAsync(int id);
    }

    public class ToDoService : IToDoService
    {
        private readonly LocalStorage ProtectedLocalStorage;

        public ToDoService(LocalStorage localStorage)
        {
            ProtectedLocalStorage = localStorage;
        }

        public async Task<List<ToDoItem>> GetAllToDosAsync()
        {
            var loginContext = await ProtectedLocalStorage.GetAsync<LoginContext>("loginContext");

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{SupabaseConfig.Url}rest/v1/todos?select=*"),
                Headers =
                {
                    {"apikey", $"{SupabaseConfig.ApiKey}"},
                    {"Authorization", $"Bearer {loginContext?.AccessToken}"},
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var items = JsonSerializer.Deserialize<List<ToDoItem>>(body, options);
                return items;
            }
        }

        public async Task InsertToDoAsync(ToDoItem toDoItem)
        {
            var loginContext = await ProtectedLocalStorage.GetAsync<LoginContext>("loginContext");
            try
            {
                Console.WriteLine(loginContext?.AccessToken);
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{SupabaseConfig.Url}rest/v1/todos"),
                    Headers =
                    {
                        {
                            "apikey",
                            $"{SupabaseConfig.ApiKey}"
                        },
                        {
                            "Authorization",
                            $"Bearer {loginContext?.AccessToken}"
                        },
                        {"Prefer", "return=representation"},
                    },
                    Content = new StringContent(
                        $"{{ \n\t\"user_id\": \"{loginContext?.User.Id}\",\n\t\"task\": \"{toDoItem.Task}\", \n\t\"is_complete\": {toDoItem.IsComplete.ToString().ToLower()} \n}}")
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task MarkAsCompleteAsync(int id)
        {
            var loginContext = await ProtectedLocalStorage.GetAsync<LoginContext>("loginContext");
            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Patch,
                RequestUri = new Uri($"{SupabaseConfig.Url}rest/v1/todos?id=eq.{id}"),
                Headers =
                {
                    {"apikey", $"{SupabaseConfig.ApiKey}"},
                    {"Authorization", $"Bearer {loginContext?.AccessToken}"},
                    {"Prefer", "return=representation"},
                },
                Content = new StringContent("{ \n\t\"is_complete\": true \n}")
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
                Console.WriteLine(body);
            }
        }
    }
}