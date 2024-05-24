using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Models;

namespace CurrentBlogs.Services.Interfaces
{
    public interface ICommentsRepository
    {

        #region Get List 
        #endregion


        #region Get Item
        #endregion

        #region Update Comments DB Item
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<Comment> UpdateCommentAsync(Comment comment);
        #endregion
    }
}
