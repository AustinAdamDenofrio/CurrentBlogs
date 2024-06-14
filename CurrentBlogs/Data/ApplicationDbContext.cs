using CurrentBlogs.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CurrentBlogs.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public virtual DbSet<ImageUpload> Images { get; set; }
        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        public IEnumerable<object> Category { get; internal set; }
    }
}
