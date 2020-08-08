using System;
using System.Collections;
using System.Threading.Tasks;
using BlogService.Db;
using Microsoft.AspNetCore.Identity;

namespace BlogService
{
    public class DbRolesInitializer : IDbInitializer
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public int Order => 1000;

        public DbRolesInitializer(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task InitializeAsync(BlogContext db)
        {
            foreach (var role in Roles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
