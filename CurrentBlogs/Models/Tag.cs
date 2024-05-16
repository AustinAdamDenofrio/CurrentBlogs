﻿using Microsoft.AspNetCore.Http.HttpResults;
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
}
