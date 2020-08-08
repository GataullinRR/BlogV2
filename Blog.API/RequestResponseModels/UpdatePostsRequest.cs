using BlogService.Db;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class UpdatePostsRequest
    {
        [Required]
        public PostUpdateData[] Posts { get; set; }

        public UpdatePostsRequest()
        {

        }
        public UpdatePostsRequest(params PostUpdateData[] posts)
        {
            Posts = posts ?? throw new ArgumentNullException(nameof(posts));
        }
    }
}
