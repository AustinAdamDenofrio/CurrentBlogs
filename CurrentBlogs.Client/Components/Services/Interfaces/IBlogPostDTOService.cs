using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Pages.AuthorMenu.BlogPosts;

namespace CurrentBlogs.Client.Components.Services.Interfaces
{
    public interface IBlogPostDTOService
    {
        Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDTO);

        Task UnpublishBlogPostAsync(int blogPostId);

        Task PublishBlogPostAsync(int blogPostId);

        Task<PagedList<BlogPostDTO>> GetPublishedBlogPostsAsync(int page, int pageSize);

        Task<PagedList<BlogPostDTO>> GetDraftPostsAsync(int page, int pageSize);

        Task<PagedList<BlogPostDTO>> GetDeletedPostsAsync(int page, int pageSize);

        Task<BlogPostDTO?> GetPublishedBlogPostBySlugAsync(string slug);

        Task<BlogPostDTO?> GetAnyBlogPostByIdAsync(int blogPostId);

        Task UpdateBlogPostAsync(BlogPostDTO blogPost);

        Task DeleteBlogPostAsync(int blogPostId);

        Task RestoreDeletedBlogPostAsync(int blogPostId);

        Task<IEnumerable<BlogPostDTO>> GetTopBlogPostsAsync(int numberOfPopular);
    }
}
