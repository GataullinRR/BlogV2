using System.Net.Http;
using System.Threading.Tasks;

namespace BlogService.API
{
    internal class BlogService : ServiceBase, IBlogService
    {
        public BlogService(HttpClient client) : base(client)
        {

        }

        public async Task<QueryPostsResponse> QueryPostsAsync(QueryPostsRequest request)
        {
            return await MakeRequestAsync<QueryPostsRequest, QueryPostsResponse>(HttpMethod.Post, BlogServiceEndpoints.QueryPosts, request);
        }

        public async Task<SignInResponse> SignInAsync(SignInRequest request)
        {
            return await MakeRequestAsync<SignInRequest, SignInResponse>(HttpMethod.Post, BlogServiceEndpoints.SignIn, request);
        }

        public async Task<SignOutResponse> SignOutAsync(SignOutRequest request)
        {
            return await MakeRequestAsync<SignOutRequest, SignOutResponse>(HttpMethod.Post, BlogServiceEndpoints.SignOut, request);
        }

        public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
        {
            return await MakeRequestAsync<SignUpRequest, SignUpResponse>(HttpMethod.Post, BlogServiceEndpoints.SignUp, request);
        }

        public async Task<UpdatePostsResponse> UpdatePostsAsync(UpdatePostsRequest request)
        {
            return await MakeRequestAsync<UpdatePostsRequest, UpdatePostsResponse>(HttpMethod.Put, BlogServiceEndpoints.UpdatePosts, request);
        }
    }
}
