using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorToDoList.Settings
{
    public class SupabaseConfig : ISupabaseConfig
    {
        private readonly IJSRuntime _jsRuntime;

        public SupabaseConfig(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetApiKey()
        {
            return await _jsRuntime.InvokeAsync<string>("process.env.Supabase_Key");
        }

        public async Task<string> GetUrl()
        {
            return await _jsRuntime.InvokeAsync<string>("process.env.Supabase_Endpoint");
        }
    }
}
