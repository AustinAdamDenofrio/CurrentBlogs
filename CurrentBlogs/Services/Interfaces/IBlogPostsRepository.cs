using CurrentBlogs.Models;

namespace CurrentBlogs.Services.Interfaces
{
    public interface IBlogPostsRepository
    {
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);

        Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();

        Task<BlogPost?> GetBlogPostByIdAsync(int blogPostId);

        Task UpdateBlogPostAsync(BlogPost blogPost);

        Task DeleteBlogPostAsync(int blogPostId);

        Task AddTagsToBlogPostAsync(int blogPostId, IEnumerable<string> tagNames);

        Task RemoveTagsFromBlogPostAsync(int blogPostId);

    }
}

