using BlogService.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class QueryPostsResponse
    {
        [Required]
        public Post[] Posts { get; set; }

        public QueryPostsResponse()
        {

        }

        public QueryPostsResponse(Post[] posts)
        {
            Posts = posts ?? throw new ArgumentNullException(nameof(posts));
        }
    }
}
