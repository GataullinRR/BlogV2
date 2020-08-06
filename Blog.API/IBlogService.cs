using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlogService.API
{
    public interface IBlogService
    {
        Task<SignOutResponse> SignOutAsync(SignOutRequest request);
        Task<SignInResponse> SignInAsync(SignInRequest request);
        Task<SignUpResponse> SignUpAsync(SignUpRequest request);
        Task<QueryPostsResponse> QueryPostsAsync(QueryPostsRequest request);
        Task<UpdatePostsResponse> UpdatePostsAsync(UpdatePostsRequest request);
        Task<CreatePostsResponse> CreatePostsAsync(CreatePostsRequest request);
    }
}
