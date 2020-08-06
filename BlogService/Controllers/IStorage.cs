using System.Threading.Tasks;
using System.IO;

namespace BlogService.Controllers
{
    public interface IStorage
    {
        Task<string> SaveImageAsync(Stream file, string extension);
    }
}
