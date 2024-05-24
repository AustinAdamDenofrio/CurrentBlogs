using CurrentBlogs.Client.Components.Pages.AuthorMenu.BlogPosts;
using CurrentBlogs.Data;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace CurrentBlogs.Services
{
    public class CommentRepository : ICommentsRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public CommentRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        #region Get List
        #endregion




        #region Get Item
        #endregion



        #region Update DB Comment Item
        public async Task CreateCommentAsync(Comment comment)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            comment.Created = DateTime.Now;
            comment.Updated = DateTime.Now;

            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            return comment;
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            bool shouldUpdate = await context.Comments.AnyAsync(bp => bp.Id == comment.Id);

            if (shouldUpdate)
            {
                context.Comments.Update(comment);
                await context.SaveChangesAsync();
            }

        }

        Task<Comment> ICommentsRepository.CreateCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        Task<Comment> ICommentsRepository.UpdateCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
