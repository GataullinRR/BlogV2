using System.Threading.Tasks;
using BlogService.Db;
using Microsoft.EntityFrameworkCore;

namespace BlogService
{
    public class DbMigrator : IDbInitializer
    {
        public int Order => 0;

        public async Task InitializeAsync(BlogContext db)
        {
            await db.Database.MigrateAsync();
        }
    }
}
