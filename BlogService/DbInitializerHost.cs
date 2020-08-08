using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlogService.Db;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlogService
{
    public class DbInitializerHost : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbInitializerHost(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var initializers = scope.ServiceProvider.GetRequiredService<IEnumerable<IDbInitializer>>();
            foreach (var initializer in initializers.OrderBy(i => i.Order))
            {
                var db = scope.ServiceProvider.GetRequiredService<BlogContext>();
                try
                {
                    await initializer.InitializeAsync(db);
                }
                finally
                {
                   // await db.DisposeAsync();
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }
}
