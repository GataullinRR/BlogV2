using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.API;
using BlogService.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PostController : ControllerBase
    {
        readonly BlogContext _db;

        public PostController(BlogContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpPost(BlogServiceEndpoints.QueryPosts)]
        public async Task<IActionResult> QueryPosts(QueryPostsRequest request)
        {
            var posts = (List<Post>)null; 
            if (request.PostIds == null)
            {
                posts = await _db.Posts
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                posts = await _db.Posts
                    .AsNoTracking()
                    .Where(p => request.PostIds.Contains(p.Id))
                    .ToListAsync();
            }

            return Ok(new QueryPostsResponse(posts));
        }

        [HttpPut(BlogServiceEndpoints.UpdatePosts)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdatePosts(UpdatePostsRequest request)
        {
            foreach (var updatedPost in request.Posts)
            {
                _db.Entry(updatedPost).State = EntityState.Modified;
            }
            await _db.SaveChangesAsync();

            return Ok(new UpdatePostsResponse());
        }
    }
}
