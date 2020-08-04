using BlogService.Db;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;

namespace BlogService.Db
{
    public class User : IdentityUser
    {
        public virtual ICollection<Post> Posts { get; set; }

        public User() { }

        public User(string userName) : base(userName)
        {

        }
    }
}
