using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Data;
using System.ComponentModel.DataAnnotations;

namespace CurrentBlogs.Models
{
    public class Comment
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
            get { return _updated; }
            set { _updated = value?.ToUniversalTime(); }
        }

        [MaxLength(200, ErrorMessage = "The {0} must be at most {1} characters long")]
        public string? UpdateReason { get; set; }

        // Database relat
        [Required]
        public string? AuthorId { get; set; }
        public virtual ApplicationUser? Author { get; set; } 

        public int BlogPostId { get; set; }
        public virtual BlogPost? BlogPost { get; set;}

    }

    public static class CommentExtensions
    {

        public static CommentDTO ToDTO(this Comment comment)
        {
            CommentDTO dto = new CommentDTO()
            {
                Id = comment.Id,
                Content = comment.Content,
                Created = comment.Created,
                Updated = comment.Updated,
                UpdateReason = comment.UpdateReason,
                AuthorId = comment.AuthorId,
                AuthorName = comment.Author?.FullName,
                //AuthorImageUrl = comment.Author.ImageId.HasValue ? $"/api/uploads/{comment.Author.ImageId}" : "",
                BlogPostId = comment.BlogPostId,
            };


            return dto;
        }
    }

}
