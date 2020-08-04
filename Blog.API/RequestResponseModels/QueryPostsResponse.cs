using BlogService.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class QueryPostsResponse
    {
        [Required]
        public List<Post> Posts { get; set; }

        public QueryPostsResponse(List<Post> posts)
        {
            Posts = posts ?? throw new ArgumentNullException(nameof(posts));
        }
    }
}
