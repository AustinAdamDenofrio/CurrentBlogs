using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Pages.AuthorMenu.BlogPosts;

namespace CurrentBlogs.Client.Components.Services.Interfaces
{
    public interface IBlogPostDTOService
    {
        Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDTO);
        Task<IEnumerable<BlogPostDTO>> GetAllBlogPostsAsync();

        Task<BlogPostDTO?> GetBlogPostByIdAsync(int blogPostId);

        Task UpdateBlogPostAsync(BlogPostDTO blogPost);

        Task DeleteBlogPostAsync(int blogPostId);
    }
}
