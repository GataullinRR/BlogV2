using System.Threading.Tasks;
using BlogService.API;
using Shared;

namespace BlogV2
{
    public interface IAuthService 
    {
        Task SignInAsync(SignInRequest request);
        Task SignOutAsync();
        Task SignUpAsync(SignUpRequest request);
        
        Task LoadTokenAsync();
    }
}
