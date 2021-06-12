using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorToDoList.Services;
using BlazorToDoList.Settings;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorToDoList
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddOptions();
            builder.Services.AddTransient<LocalStorage>();
            builder.Services.AddTransient<Authentication>();
            builder.Services.AddTransient<IToDoService, ToDoService>();

            builder.Services.AddScoped(
                sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

            await builder.Build().RunAsync();
        }
    }
}