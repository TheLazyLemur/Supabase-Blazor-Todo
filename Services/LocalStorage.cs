using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorToDoList.Services
{
    public class LocalStorage
    {
        public IJSRuntime JsRuntime { get; set; }

        public LocalStorage(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        public async Task SetAsync(string key, string value)
        {
            await JsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        public async Task<string> GetAsync(string key)
        {
            return await JsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }

        public async Task RemoveAsync(string key)
        {
            await JsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public async Task ClearAsync()
        {
            await JsRuntime.InvokeVoidAsync("localStorage.clear");
        }
    }
}