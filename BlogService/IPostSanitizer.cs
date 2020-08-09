using System.Threading.Tasks;

namespace BlogService.Controllers
{
    public interface IPostSanitizer
    {
        Task<string> SanitizeAsync(string body, bool escapeExecutable);
        string IgnoreNonTextNodes(string richHtml);
    }
}
