using System.Xml.Linq;
using CurrentBlogs.Client.Components.Models;

namespace CurrentBlogs.Client.Components.Services.Interfaces
{
    public interface ICommentsDTOService
    {
        #region Get List 
        Task<IEnumerable<CommentDTO>> GetCommentsByBlogPostIdAsync(int blogPostId);
        #endregion


        #region Get Item
        Task<CommentDTO?> GetCommentByIdAsync(int commentId);

        #endregion

        #region Update Comments DB Item
        Task CreateCommentAsync(CommentDTO comment, string userId);
        Task UpdateCommentAsync(CommentDTO comment, string userId);
        Task DeleteCommentAsync(int commentId);
        #endregion
    }
}
