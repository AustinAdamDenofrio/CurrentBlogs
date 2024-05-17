using CurrentBlogs.Client.Components.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace CurrentBlogs.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long", MinimumLength = 2)]
        public string? Name { get; set; }

        // DB Relationationship

        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new HashSet<BlogPost>();
    }

    public static class TagExtensions
    {
        public static TagDTO ToDTO(this Tag tag)
        {
            TagDTO dto = new TagDTO()
            {
                Id = tag.Id,
                Name = tag.Name,
            };

            foreach (BlogPost blogPost in tag.BlogPosts)
            {
                dto.BlogPosts.Add(blogPost.ToDTO());
            }

            return dto;
        }
    }
}

