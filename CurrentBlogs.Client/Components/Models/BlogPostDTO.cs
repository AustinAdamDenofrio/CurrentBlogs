using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CurrentBlogs.Client.Components.Models
{
    public class BlogPostDTO
    {

        private DateTimeOffset _created;
        private DateTimeOffset? _updated;
        public int Id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} at most {1} characters long", MinimumLength = 2)]
        public string? Title { get; set; }

        public string? Slug { get; set; }

        [Required]
        [StringLength(600, ErrorMessage = "The {0} must be at least {2} at most {1} characters long", MinimumLength = 2)]
        public string? Abstract { get; set; }

        [Required]
        public string? Content { get; set; }

        [Required]
        public DateTimeOffset Created
        {
            get => _created.ToLocalTime();
            set => _created = value.ToUniversalTime();
        }
        public DateTimeOffset? Updated
        {
            get { return _updated?.ToLocalTime(); }
            set { _updated = value?.ToUniversalTime(); }
        }

        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }

                 //Database Relationships/Navigation Poperties
        public string? ImageUrl { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "A category must be selected.")]
        public int CategoryId { get; set; }
        public CategoryDTO? Category { get; set; }

        public ICollection<CommentDTO> Comments { get; set; } = new HashSet<CommentDTO>();

        public ICollection<TagDTO> Tags { get; set; } = new HashSet<TagDTO>();

    }
}
