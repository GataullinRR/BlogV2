using System.Threading.Tasks;
using BlogService.Db;

namespace BlogService
{
    public interface IDbInitializer
    {
        int Order { get; }

        Task InitializeAsync(BlogContext db);
    }
}
