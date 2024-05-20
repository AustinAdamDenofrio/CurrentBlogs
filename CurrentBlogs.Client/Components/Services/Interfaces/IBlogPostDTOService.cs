using CurrentBlogs.Client.Components.Models;

namespace CurrentBlogs.Client.Components.Services.Interfaces
{
    public interface IBlogPostDTOService
    {
        Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDTO);
    }
}
