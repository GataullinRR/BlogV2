using System;
using System.Threading.Tasks;
using System.IO;

namespace BlogService.Controllers
{
    public class Storage : IStorage
    {
        public Storage()
        {

        }

        public async Task<string> SaveImageAsync(Stream file, string extension)
        {
            var serverLocalPath = Path.Combine("images", "users", "posts", $"{Guid.NewGuid()}.{extension}");
            var serverPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", serverLocalPath);
            var dir = Path.GetDirectoryName(serverPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (var toServerStream = File.Open(serverPath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(toServerStream);
            }

            return serverLocalPath;
        }
    }
}
