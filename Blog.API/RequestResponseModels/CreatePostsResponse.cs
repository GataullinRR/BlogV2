using System;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class CreatePostsResponse
    {
        [Required]
        public int[] Ids { get; set; }

        public CreatePostsResponse()
        {

        }

        public CreatePostsResponse(int[] ids)
        {
            Ids = ids ?? throw new ArgumentNullException(nameof(ids));
        }
    }
}
