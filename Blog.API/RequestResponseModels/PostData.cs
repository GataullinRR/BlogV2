using BlogService.Db;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class PostData
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public string? BodyPreview { get; set; }

        public PostData()
        {

        }

        public PostData(string title, string body, string bodyPreview)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Body = body ?? throw new ArgumentNullException(nameof(body));
            BodyPreview = bodyPreview;
        }
    }
}
