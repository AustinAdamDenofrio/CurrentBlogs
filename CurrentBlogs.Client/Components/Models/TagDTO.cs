using System.ComponentModel.DataAnnotations;

namespace CurrentBlogs.Client.Components.Models
{
    public class TagDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long", MinimumLength = 2)]
        public string? Name { get; set; }

        // DB Relationationship
        public ICollection<BlogPostDTO> BlogPosts { get; set; } = new HashSet<BlogPostDTO>();
    }
}
