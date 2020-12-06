using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using BlogService.API;
using Microsoft.EntityFrameworkCore.Internal;
using Shared;

namespace BlogV2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var services = builder.Services;
            //builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            services.AddBlazoredLocalStorage();
            services.AddOptions();
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<AuthTokenProvider>();
            services.AddTransient<IAuthTokenProvider>(sp => sp.GetRequiredService<AuthTokenProvider>());
            services.AddBlogService(builder.Configuration);
            services.PostConfigure<BlogServiceOptions>(o =>
            {
                var hostedUri = new Uri(builder.HostEnvironment.BaseAddress);
                var apiUriBilder = new UriBuilder(hostedUri);
                apiUriBilder.Port = builder.Configuration.GetValue<int>("Services:API:Port");
                o.Address = apiUriBilder.Uri;
            });

            await builder.Build().RunAsync();
        }

    }
}
