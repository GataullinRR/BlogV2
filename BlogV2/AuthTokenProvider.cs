using Shared;

namespace BlogV2
{
    public class AuthTokenProvider : IAuthTokenProvider
    {
        public string? Token { get; set; }
    }
}
