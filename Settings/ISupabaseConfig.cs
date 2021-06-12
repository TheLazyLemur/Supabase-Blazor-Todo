using System.Threading.Tasks;

namespace BlazorToDoList.Settings
{
    public interface ISupabaseConfig
    {
        public Task<string> GetApiKey();
        public Task<string> GetUrl();
    }
}