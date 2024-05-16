using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Models;
namespace CurrentBlogs.Models
{
    public class BlogPost
    {


        private DateTimeOffset _created;
        private DateTimeOffset? _updated;
        public int Id { get; set; }
        
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} at most {1} characters long", MinimumLength = 2)]
        public string? Title { get; set; }

        [Required]
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
            get { return _updated; }
            set { _updated = value?.ToUniversalTime(); }
        }

        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }

        //Database Relationships/Navigation Poperties

        public Guid? ImageId { get; set; }
        public virtual ImageUpload? Image { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<Tag> Tags { get; set; }  = new HashSet<Tag>();
    }

    //public static class BlogPostExtensions
    //{

    //    public static BlogPostDTO ToDTO(this BlogPost blogPost)
    //    {
    //        BlogPostDTO dto = new BlogPostDTO()
    //        {
    //            Id = blogPost.Id,
    //            FirstName = blogPost.FirstName,

    //            ImageUrl = blogPost.ImageId.HasValue ? $"/api/uploads/{blogPost.ImageId}" : UploadHelper.DefaultContactImage,
    //        };

    //        //ToDo: Categories

    //        foreach (Category category in contact.Categories)
    //        {
    //            category.Contacts.Clear();
    //            dto.Categories.Add(category.ToDTO());
    //        }

    //        return dto;
    //    }
    //}
}
