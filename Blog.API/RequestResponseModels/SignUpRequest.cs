using System;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class SignUpRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string EMail { get; set; }

        [Required]
        public string Password { get; set; }

        public SignUpRequest(string userName, string eMail, string password)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            EMail = eMail ?? throw new ArgumentNullException(nameof(eMail));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
}
