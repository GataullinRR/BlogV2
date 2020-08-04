using BlogService.Db;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class UpdatePostsRequest
    {
        [Required]
        public Post[] Posts { get; set; }

        public UpdatePostsRequest(Post[] posts)
        {
            Posts = posts ?? throw new ArgumentNullException(nameof(posts));
        }
    }
}
