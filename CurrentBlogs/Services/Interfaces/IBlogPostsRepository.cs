using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Models;

namespace CurrentBlogs.Services.Interfaces
{
    public interface IBlogPostsRepository
    {

        #region Get List
        Task<PagedList<BlogPost>> GetPublishedBlogPostsAsync(int page, int pageSize);
        Task<PagedList<BlogPost>> GetDraftPostsAsync(int page, int pageSize);
        Task<PagedList<BlogPost>> GetDeletedPostsAsync(int page, int pageSize);
        Task<PagedList<BlogPost>> GetPostsByCategoryId(int categoryId, int page, int pageSize);
        Task<PagedList<BlogPost>> SearchBlogPostsAsync(string query, int page, int pageSize);

        Task<IEnumerable<BlogPost>> GetTopBlogPostsAsync(int numberOfPopular);
        #endregion




        #region Get Item
        Task<BlogPost?> GetAnyBlogPostByIdAsync(int blogPostId);
        Task<BlogPost?> GetPublishedBlogPostBySlugAsync(string slug);

        Task<Tag?> GetTagByIdAsync(int tagId);
        Task<PagedList<BlogPost>> GetPostsByTagIdAsync(int tagId, int page, int pageSize);
        #endregion




        #region Update BlogPost DB item
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);
        Task UpdateBlogPostAsync(BlogPost blogPost);
        Task PublishBlogPostAsync(int blogPostId);
        Task UnpublishBlogPostAsync(int blogPostId);
        Task DeleteBlogPostAsync(int blogPostId);
        Task RestoreDeletedBlogPostAsync(int blogPostId);
        #endregion


        #region Update Tags DB Item
        Task AddTagsToBlogPostAsync(int blogPostId, IEnumerable<string> tagNames);
        Task RemoveTagsFromBlogPostAsync(int blogPostId);
        #endregion

    }
}

