using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Pages.AuthorMenu.BlogPosts;

namespace CurrentBlogs.Client.Components.Services.Interfaces
{
    public interface IBlogPostDTOService
    {
        Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDTO);

        Task<PagedList<BlogPostDTO>> GetPublishedBlogPostsAsync(int page, int pageSize);

        Task<BlogPostDTO?> GetBlogPostBySlugAsync(string slug);

        Task<BlogPostDTO?> GetBlogPostByIdAsync(int blogPostId);

        Task UpdateBlogPostAsync(BlogPostDTO blogPost);

        Task DeleteBlogPostAsync(int blogPostId);

        Task<IEnumerable<BlogPostDTO>> GetTopBlogPostsAsync(int numberOfPopular);
    }
}
