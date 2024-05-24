using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Models;

namespace CurrentBlogs.Services.Interfaces
{
    public interface IBlogPostsRepository
    {
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);

        Task UnpublishBlogPostAsync(int blogPostId);

        Task PublishBlogPostAsync(int blogPostId);

        Task<PagedList<BlogPost>> GetPublishedBlogPostsAsync(int page, int pageSize);

        Task<PagedList<BlogPost>> GetDraftPostsAsync(int page, int pageSize);

        Task<PagedList<BlogPost>> GetDeletedPostsAsync(int page, int pageSize);

        Task<BlogPost?> GetAnyBlogPostByIdAsync(int blogPostId);

        Task UpdateBlogPostAsync(BlogPost blogPost);

        Task DeleteBlogPostAsync(int blogPostId);

        Task RestoreDeletedBlogPostAsync(int blogPostId);

        Task<BlogPost?> GetPublishedBlogPostBySlugAsync(string slug);

        Task AddTagsToBlogPostAsync(int blogPostId, IEnumerable<string> tagNames);

        Task RemoveTagsFromBlogPostAsync(int blogPostId);

        Task<IEnumerable<BlogPost>> GetTopBlogPostsAsync(int numberOfPopular);

    }
}

