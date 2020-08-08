using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.API;
using BlogService.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogService.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PostController : ControllerBase
    {
        readonly BlogContext _db;
        readonly IPostSanitizer _sanitizer;

        public PostController(BlogContext db, IPostSanitizer sanitizer)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _sanitizer = sanitizer ?? throw new ArgumentNullException(nameof(sanitizer));
        }

        [HttpPost(BlogServiceEndpoints.QueryPosts)]
        public async Task<IActionResult> QueryPosts([FromBody]QueryPostsRequest request)
        {
            var posts = (List<Post>)null; 
            if (request.PostIds == null)
            {
                posts = await _db.Posts
                    .Include(p => p.Author)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                posts = await _db.Posts
                    .Include(p => p.Author)
                    .AsNoTracking()
                    .Where(p => request.PostIds.Contains(p.Id))
                    .ToListAsync();
            }

            foreach (var post in posts)
            {
                post.Author.Posts = new Post[0];
            }

            return Ok(new QueryPostsResponse(posts));
        }

        [HttpPut(BlogServiceEndpoints.UpdatePosts)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdatePosts([FromBody]UpdatePostsRequest request)
        {
            await PreparePostDataAsync(request.Posts);
            var ids = request.Posts.Select(p => p.Id).ToArray();
            var posts = await _db.Posts
                .Where(p => ids.Contains(p.Id))
                .ToArrayAsync();
            foreach (var post in posts)
            {
                var newPost = request.Posts.First(p => p.Id == post.Id);
                post.Title = newPost.Title;
                post.Body = newPost.Body;
                post.BodyPreview = newPost.BodyPreview;
            }
            await _db.SaveChangesAsync();

            return Ok(new UpdatePostsResponse());
        }

        [HttpPost(BlogServiceEndpoints.CreatePosts)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreatePosts([FromBody]CreatePostsRequest request, [FromServices]UserManager<User> userManager)
        {
            await PreparePostDataAsync(request.Posts);
            var author = await userManager.FindByNameAsync(User.Identity.Name);
            var posts = request.Posts
                .Select(d => new Post(DateTime.UtcNow, author, d.Title, d.Body, d.BodyPreview))
                .ToArray();
            await _db.Posts.AddRangeAsync(posts);
            await _db.SaveChangesAsync();

            return Ok(new CreatePostsResponse(posts.Select(p => p.Id).ToArray()));
        }

        private async Task PreparePostDataAsync(PostData[] datas)
        {
            foreach (var data in datas)
            {
                data.Body = await _sanitizer.SanitizeAsync(data.Body);
                if (data.BodyPreview == null)
                {
                    data.BodyPreview = _sanitizer.IgnoreNonTextNodes(data.Body);
                    data.BodyPreview = new string(data.BodyPreview.Take(500).ToArray()) 
                        + ((data.BodyPreview.Length > 500) ? "..." : "");
                }
                else
                {
                    data.BodyPreview = await _sanitizer.SanitizeAsync(data.BodyPreview);
                    _sanitizer.IgnoreNonTextNodes(data.BodyPreview);
                }
            }
        }
    }
}
