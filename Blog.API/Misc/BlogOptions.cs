using System;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class BlogServiceOptions
    {
        [Required]
        public Uri Address { get; set; } = new Uri("https://localhost:5010");
    }
}
