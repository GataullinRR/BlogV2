using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlogService.API;
using BlogService.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BlogService.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BlogContext _db;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthController(BlogContext db, IConfiguration configuration, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost(BlogServiceEndpoints.SignIn)]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByNameAsync(request.UserName);
                var roles = await _signInManager.UserManager.GetRolesAsync(user);
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, request.UserName));

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

                var token = new JwtSecurityToken(
                    _configuration["JwtIssuer"],
                    _configuration["JwtAudience"],
                    claims,
                    expires: expiry,
                    signingCredentials: creds
                );

                return Ok(new SignInResponse(new JwtSecurityTokenHandler().WriteToken(token)));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(BlogServiceEndpoints.SignOut)]
        public async Task<IActionResult> SignOut(SignOutRequest request)
        {
#warning Implement!
            return Ok(new SignOutResponse());
        }

        [HttpPost(BlogServiceEndpoints.SignUp)]
        public async Task<IActionResult> SignUp(SignUpRequest request)
        {
            var newUser = new User(request.UserName)
            {
                Email = request.EMail
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return BadRequest();
            }
            else
            {
                if (await _db.Users.CountAsync() == 1)
                {
                    await _userManager.AddToRoleAsync(newUser, Roles.Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, Roles.User);
                }
                    
                return Ok(new SignOutResponse());
            }
        }
    }
}
