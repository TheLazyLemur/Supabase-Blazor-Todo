using BlazorToDoList.Data;
using BlazorToDoList.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorToDoList.Pages
{
    public partial class Index
    {
        private User User { get; set; }
        private LoginContext LoginContext { get; set; }
        [Inject] public LocalStorage ProtectedLocalStorage { get; set; }

        private enum ApplicationState
        {
            SignedOut = 1,
            LoggedIn = 2
        }

        private ApplicationState _currentApplicationState = ApplicationState.SignedOut;

        private void OnUserLoggedIn(LoginContext loginContext)
        {
            User = loginContext.User;
            _currentApplicationState = ApplicationState.LoggedIn;
            LoginContext = loginContext;
            StateHasChanged();
        }

        private async void OnUserLoggedOut()
        {
            await ProtectedLocalStorage.ClearAsync();
            _currentApplicationState = ApplicationState.SignedOut;
            StateHasChanged();
        }
    }
}