using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BlogService.Db
{
    public class Post
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public DateTime CreationTime { get; set; }

        [Required] 
        [InverseProperty(nameof(User.Posts))]
        public User Author { get; set; }

        [Required] 
        public string Title { get; set; }

        [Required] 
        public string Body { get; set; }

        [Required] 
        public string BodyPreview { get; set; }

        public Post()
        {

        }

        public Post(DateTime creationTime, User author, string title, string body, string bodyPreview)
        {
            CreationTime = creationTime;
            Author = author ?? throw new ArgumentNullException(nameof(author));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Body = body ?? throw new ArgumentNullException(nameof(body));
            BodyPreview = bodyPreview ?? throw new ArgumentNullException(nameof(bodyPreview));
        }
    }
}
