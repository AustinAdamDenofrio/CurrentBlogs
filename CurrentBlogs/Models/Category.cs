using CurrentBlogs.Client.Components.Models;
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

    public static class CategoryExtensions
    {

        public static CategoryDTO ToDTO(this Category category)
        {
            CategoryDTO dto = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
    //            ImageUrl = category.ImageId.HasValue ? $"/api/uploads/{category.ImageId}" : "",
            };

            //ToDo: BlogPosts
            foreach (BlogPost blogPost in category.BlogPosts)
            {
                dto.BlogPosts.Add(blogPost.ToDTO());
            }

            return dto;
        }

    }
}
