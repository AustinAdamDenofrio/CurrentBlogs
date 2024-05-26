using System.Xml.Linq;
using CurrentBlogs.Client.Components.Models;

namespace CurrentBlogs.Client.Components.Services.Interfaces
{
    public interface ICommentsDTOService
    {
        #region Get List 
        Task<PagedList<CommentDTO>> GetCommentsByBlogPostIdAsync(int blogPostId, int page, int pageSize);
        #endregion


        #region Get Item
        Task<CommentDTO?> GetCommentById(CommentDTO commentDTO, int userId);

        #endregion

        #region Update Comments DB Item
        Task CreateCommentAsync(CommentDTO comment, string userId);
        Task UpdateCommentAsync(CommentDTO comment, string userID);
        Task DeleteCommentAsync(int commentId, string userId);
        #endregion
    }
}
