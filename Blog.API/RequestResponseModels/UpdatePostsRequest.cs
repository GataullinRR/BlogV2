using BlogService.Db;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlogService.API
{
    public class UpdatePostsRequest
    {
        [Required]
        public IPostUpdateRequest[] UpdateRequests { get; set; }

        public UpdatePostsRequest()
        {

        }

        public UpdatePostsRequest(params IPostUpdateRequest[] updateRequests)
        {
            UpdateRequests = updateRequests ?? throw new ArgumentNullException(nameof(updateRequests));
        }
    }
}
