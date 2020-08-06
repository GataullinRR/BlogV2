using BlogService.Db;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class CreatePostsRequest
    {
        [Required]
        public PostData[] Posts { get; set; }

        public CreatePostsRequest()
        {

        }

        public CreatePostsRequest(params PostData[] posts)
        {
            Posts = posts ?? throw new ArgumentNullException(nameof(posts));
        }
    }
}
