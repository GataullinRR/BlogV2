using System.Threading.Tasks;

namespace BlogService.Controllers
{
    public interface IPostSanitizer
    {
        Task<string> SanitizeAsync(string body);
        string IgnoreNonTextNodes(string richHtml);
    }
}
