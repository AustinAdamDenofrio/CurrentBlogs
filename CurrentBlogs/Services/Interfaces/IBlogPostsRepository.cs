using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Models;

namespace CurrentBlogs.Services.Interfaces
{
    public interface IBlogPostsRepository
    {
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);

        Task<PagedList<BlogPost>> GetPublishedBlogPostsAsync(int page, int pageSize);

        Task<BlogPost?> GetBlogPostByIdAsync(int blogPostId);

        Task UpdateBlogPostAsync(BlogPost blogPost);

        Task DeleteBlogPostAsync(int blogPostId);

        Task<BlogPost?> GetBlogPostBySlugAsync(string slug);

        Task AddTagsToBlogPostAsync(int blogPostId, IEnumerable<string> tagNames);

        Task RemoveTagsFromBlogPostAsync(int blogPostId);

        Task<IEnumerable<BlogPost>> GetTopBlogPostsAsync(int numberOfPopular);

    }
}

