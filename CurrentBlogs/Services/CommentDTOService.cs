using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Services.Interfaces;
using CurrentBlogs.Helper;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using NuGet.Protocol.Core.Types;

namespace CurrentBlogs.Services
{
    public class CommentDTOService : ICommentsDTOService
    {
        private readonly ICommentsRepository _repository;
        public CommentDTOService(ICommentsRepository repository)
        {
            _repository = repository;
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
        Task<CommentDTO?> ICommentsDTOService.GetCommentById(CommentDTO commentDTO, int userId)
        {
            throw new NotImplementedException();
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
            Comment? comment = await _repository.GetCommentById(commentDTO.Id, userId);

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

        public async Task DeleteCommentAsync(int commentId, string userId)
        {
            await _repository.DeleteCommentAsync(commentId, userId);
        }

        #endregion

    }
}
