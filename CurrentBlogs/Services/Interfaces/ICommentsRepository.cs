using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Models;

namespace CurrentBlogs.Services.Interfaces
{
    public interface ICommentsRepository
    {

        #region Get List 
        Task<PagedList<Comment>> GetCommentsByBlogPostIdAsync(int blogPostId, int page, int pageSize);
        #endregion


        #region Get Item
        Task<Comment?> GetCommentById(int commentId, string userId);
        #endregion

        #region Update Comments DB Item
        Task CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId, string userId);
        #endregion
    }
}
