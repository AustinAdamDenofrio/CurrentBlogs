using CurrentBlogs.Client.Components.Helper;
using CurrentBlogs.Client.Components.Pages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CurrentBlogs.Client.Components.Models
{
    public class CommentDTO
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;
        public int Id { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} at most {1} characters long", MinimumLength = 2)]
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

        [MaxLength(200, ErrorMessage = "The {0} must be at most {1} characters long")]
        public string? UpdateReason { get; set; }


        public string? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorImageUrl { get; set; }
        public int BlogPostId { get; set; }
    }
}
