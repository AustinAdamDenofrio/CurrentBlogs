using System.Xml.Linq;
using CurrentBlogs.Client.Components.Models;

namespace CurrentBlogs.Client.Components.Services.Interfaces
{
    public interface ICommentsDTOService
    {
        #region Get List 
        #endregion


        #region Get Item

        #endregion

        #region Update Comments DB Item
        Task CreateCommentAsync(CommentDTO comment, string UserId);
        Task UpdateCommentAsync(CommentDTO comment, string UserId);
        #endregion
    }
}
