using CurrentBlogs.Models;

namespace CurrentBlogs.Services.Interfaces
{
    public interface IBlogPostsRepository
    {
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);
    }
}
