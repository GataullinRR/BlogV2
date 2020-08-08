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
        public bool IsHidden { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required] 
        public DateTime CreationTime { get; set; }

        // Yes, no migrations, but inconvenient
        //[Required]
        //public List<PostAttribute> Attributes { get; set; } = new List<PostAttribute>();

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

    //public class PostAttribute
    //{
    //    public int Id { get; set; }

    //    public string Name { get; set; }
    //    public string Value { get; set; }
    //}
}
