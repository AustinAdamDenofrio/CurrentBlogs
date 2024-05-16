using System.ComponentModel.DataAnnotations;
using CurrentBlogs.Client.Components.Helper;

namespace CurrentBlogs.Client.Components.Models
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long", MinimumLength = 2)]
        public string? Name { get; set; }


        [MaxLength(200, ErrorMessage = "The {0} must be at most {1} characters long")]
        public string? Description { get; set; }

        //Database Relationships

        public string? ImageUrl { get; set; }  

        public ICollection<BlogPostDTO> BlogPosts { get; set; } = new HashSet<BlogPostDTO>();

    }
}
