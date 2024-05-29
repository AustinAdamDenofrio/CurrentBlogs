using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Services.Interfaces;
using CurrentBlogs.Helper;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Identity;
using CurrentBlogs.Data;

namespace CurrentBlogs.Services
{
    public class CommentDTOService : ICommentsDTOService
    {
        private readonly ICommentsRepository _repository;
        //private readonly UserManager<ApplicationUser> _userManager;

        public CommentDTOService(ICommentsRepository repository)
        {
            _repository = repository;
            //_userManager = userManager;
        }


        #region Get List of Items
        public async Task<IEnumerable<CommentDTO>> GetCommentsByBlogPostIdAsync(int blogPostId)
        {
            IEnumerable<Comment> comments = await _repository.GetCommentsByBlogPostIdAsync(blogPostId);

            IEnumerable<CommentDTO> commentsDTO = comments.Select(c => c.ToDTO());

            return commentsDTO;
        }
        #endregion


        #region Get Item
        public async Task<CommentDTO?> GetCommentByIdAsync(int commentId)
        {
            Comment? comment = await _repository.GetCommentByIdAsync(commentId);
            return comment == null ? null : comment.ToDTO();
        }
        #endregion


        #region Update db Item
        public async Task CreateCommentAsync(CommentDTO comment, string userId)
        {
            Comment newComment = new Comment()
            {
                Content = comment.Content,
                AuthorId = userId,
                BlogPostId = comment.BlogPostId,
            };

            await _repository.CreateCommentAsync(newComment);
        }

        public async Task UpdateCommentAsync(CommentDTO commentDTO, string userId)
        {
            Comment? comment = await _repository.GetCommentByIdAsync(commentDTO.Id);

            if (comment is not null)
            {
                comment.Updated = DateTimeOffset.Now;
                comment.AuthorId = userId;
                comment.Content = commentDTO.Content;
                comment.UpdateReason = commentDTO.UpdateReason;
                comment.BlogPostId = commentDTO.BlogPostId;

                await _repository.UpdateCommentAsync(comment);
            }
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            await _repository.DeleteCommentAsync(commentId);
        }

        #endregion

    }
}
