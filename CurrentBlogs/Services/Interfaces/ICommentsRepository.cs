using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Models;

namespace CurrentBlogs.Services.Interfaces
{
    public interface ICommentsRepository
    {

        #region Get List 
        Task<IEnumerable<Comment>> GetCommentsByBlogPostIdAsync(int blogPostId);
        #endregion


        #region Get Item
        Task<Comment?> GetCommentByIdAsync(int commentId);
        #endregion

        #region Update Comments DB Item
        Task CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId);
        #endregion
    }
}
