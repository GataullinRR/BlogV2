using System;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class SignInResponse
    {
        [Required]
        public string Token { get; set; }

        public SignInResponse(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}
