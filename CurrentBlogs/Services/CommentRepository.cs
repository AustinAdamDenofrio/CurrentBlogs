using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Pages.AuthorMenu.BlogPosts;
using CurrentBlogs.Data;
using CurrentBlogs.Helper.Extensions;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Drawing.Printing;

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
        public async Task<PagedList<Comment>> GetCommentsByBlogPostIdAsync(int blogPostId, int page, int pageSize)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();


            PagedList<Comment> comments = await context.Comments
                                                .Where(c => c.BlogPost!.Id == blogPostId)
                                                .Include(c => c.Author)
                                                .OrderBy(c => c.Created)
                                                .ToPagedListAsync(page, pageSize);
            return comments;
        }
        #endregion




        #region Get Item
        public async Task<Comment?> GetCommentById(int commentId, string userId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            Comment? comment = await context.Comments
                                    .Include(c => c.BlogPost)
                                    .Include(c => c.Author)
                                    .FirstOrDefaultAsync(c => c.Id == commentId && c.AuthorId == userId);



            return comment;
        }
        #endregion



        #region Update DB Comment Item
        public async Task CreateCommentAsync(Comment comment)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            comment.Created = DateTime.Now;
            comment.Updated = DateTime.Now;

            context.Comments.Add(comment);
            await context.SaveChangesAsync();
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

        public async Task DeleteCommentAsync(int commentId, string userId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            Comment? comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.AuthorId == userId);

            if (comment != null)
            {
                context.Comments.Remove(comment);
                await context.SaveChangesAsync();
            }
        }

        #endregion
    }
}
