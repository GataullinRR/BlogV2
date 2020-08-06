using Blazored.LocalStorage;
using Shared;
using System;
using System.Threading.Tasks;

namespace BlogV2
{
    public class AuthTokenProvider : IAuthTokenProvider
    {
        public string? Token { get; set; }
    }
}
