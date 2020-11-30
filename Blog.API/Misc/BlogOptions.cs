using System;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class BlogServiceOptions
    {
        [Required]
        public Uri Address { get; set; } = new Uri("http://ec2-35-181-48-229.eu-west-3.compute.amazonaws.com:80/");
    }
}
