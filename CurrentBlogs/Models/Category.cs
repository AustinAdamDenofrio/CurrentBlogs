using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace CurrentBlogs.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long", MinimumLength = 2)]
        public string? Name { get; set; }


        [MaxLength(200, ErrorMessage = "The {0} must be at most {1} characters long")]
        public string? Description { get; set; }

        //Database Relationships

        public Guid? ImageId { get; set; }
        public virtual ImageUpload? Image { get; set; } 

        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new HashSet<BlogPost>();


    }
}
