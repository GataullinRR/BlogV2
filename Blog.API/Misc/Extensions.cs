using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared;
using System.Net.Http.Headers;

namespace BlogService.API
{
    public static class Extensions
    {
        public static IServiceCollection AddBlogService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlogServiceOptions>(configuration);
            services.AddHttpClient<IBlogService, BlogService>((sp, c) =>
            {
                var options = sp.GetRequiredService<IOptions<BlogServiceOptions>>().Value;
                var token = sp.GetRequiredService<IAuthTokenProvider>().Token;
                if (token != null)
                {
                    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
                c.BaseAddress = options.Address;
            });

            return services;
        }
    }
}
