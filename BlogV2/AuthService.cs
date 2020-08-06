using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using BlogService.API;
using Shared;

namespace BlogV2
{
    public class AuthService : IAuthService 
    {
        private readonly IBlogService _blog;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly AuthTokenProvider _tokenProvider;

        public AuthService(IBlogService blog, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider, AuthTokenProvider tokenProvider)
        {
            _blog = blog ?? throw new ArgumentNullException(nameof(blog));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }

        public async Task LoadTokenAsync()
        {
            _tokenProvider.Token = await _localStorage.GetItemAsync<string>("authToken");
        }

        public async Task SignInAsync(SignInRequest request)
        {
            var result = await _blog.SignInAsync(request);
            _tokenProvider.Token = result.Token;
            await _localStorage.SetItemAsync("authToken", _tokenProvider.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(_tokenProvider.Token);
        }

        public async Task SignOutAsync()
        {
            _tokenProvider.Token = null;
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }

        public async Task SignUpAsync(SignUpRequest request)
        {
            await _blog.SignUpAsync(request);
        }
    }
}
