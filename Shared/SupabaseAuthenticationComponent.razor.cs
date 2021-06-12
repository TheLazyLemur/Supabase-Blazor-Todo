using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorToDoList.Data;
using BlazorToDoList.Extensions;
using BlazorToDoList.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorToDoList.Shared
{
    public partial class SupabaseAuthenticationComponent
    {
        protected internal class UserLogin
        {
            [Required] [EmailAddress] public string Email { get; set; }
            [Required] public string Password { get; set; }
        }

        private UserLogin _userLogin = new();
        private bool _rememberMe = true;

        [Parameter] public EventCallback<LoginContext> OnUserLoggedIn { get; set; }

        [Inject] public LocalStorage LocalStorage { get; set; }

        private enum LoginState
        {
            Waiting,
            Authenticating,
            Authenticated
        }

        private LoginState _currentLoginState = LoginState.Waiting;

        private enum PageState
        {
            Login,
            Register
        }

        private PageState _pageState = PageState.Login;

        protected override async Task OnInitializedAsync()
        {
            var storedCreds = await LocalStorage.GetAsync("email&password");
            
            if(string.IsNullOrEmpty(storedCreds)) return;

            var emailAndPassword = storedCreds.DecodeBase64().Split(":");
            var storedEmail = emailAndPassword[0];
            var storedPassword = emailAndPassword[1];

            if (!string.IsNullOrEmpty(storedEmail) && !string.IsNullOrEmpty(storedPassword))
            {
                _currentLoginState = LoginState.Authenticating;
                StateHasChanged();

                var loginContext = await _authentication.LoginAsync(storedEmail, storedPassword);
                _currentLoginState = LoginState.Authenticated;
                StateHasChanged();

                await OnUserLoggedIn.InvokeAsync(loginContext);
            }
        }

        private async void HandleValidLoginSubmit()
        {
            _currentLoginState = LoginState.Authenticating;
            var loginContext = await _authentication.LoginAsync(_userLogin.Email, _userLogin.Password);
            var jsonContext = JsonSerializer.Serialize(loginContext);
            await LocalStorage.SetAsync("loginContext", jsonContext);

            if (_rememberMe)
            {
                await LocalStorage.SetAsync("email&password",
                    $"{_userLogin.Email}:{_userLogin.Password}".EncodeBase64());
            }

            await OnUserLoggedIn.InvokeAsync(loginContext);
        }

        private async void SetToRegister()
        {
            _pageState = PageState.Register;
            StateHasChanged();
        }

        public async void SetToLogin()
        {
            _pageState = PageState.Login;
            StateHasChanged();
        }

        private async Task HandleValidRegSubmit()
        {
            await _authentication.Register(_userLogin.Email, _userLogin.Password);
            StateHasChanged();
        }
    }
}