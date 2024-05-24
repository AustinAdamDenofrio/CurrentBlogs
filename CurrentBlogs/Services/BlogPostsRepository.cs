using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Pages.AuthorMenu;
using CurrentBlogs.Client.Components.Pages.AuthorMenu.BlogPosts;
using CurrentBlogs.Data;
using CurrentBlogs.Helper.Extensions;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CurrentBlogs.Services
{

    public class BlogPostsRepository : IBlogPostsRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public BlogPostsRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }



        public async Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            blogPost.Created = DateTime.Now;
            blogPost.Updated = DateTime.Now;

            blogPost.Slug = await GenerateSlugAsync(blogPost.Title!, blogPost.Id);

            context.Add(blogPost);

            await context.SaveChangesAsync();
            return blogPost;
        }


        #region Get List
        public async Task<PagedList<BlogPost>> GetPublishedBlogPostsAsync(int page, int pageSize)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            PagedList<BlogPost> posts = await context.BlogPosts
                                                            .Where(b => b.IsPublished && !b.IsDeleted)
                                                            .Include(b => b.Category)
                                                            .OrderByDescending(bp => bp.Created)
                                                            .ToPagedListAsync(page, pageSize);

            return posts;
        }
        public async Task<PagedList<BlogPost>> GetDraftPostsAsync(int page, int pageSize)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            PagedList<BlogPost> posts = await context.BlogPosts
                                                            .Where(b => !b.IsPublished && !b.IsDeleted)
                                                            .Include(b => b.Category)
                                                            .OrderByDescending(bp => bp.Created)
                                                            .ToPagedListAsync(page, pageSize);

            return posts;
        }
        public async Task<PagedList<BlogPost>> GetDeletedPostsAsync(int page, int pageSize)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            PagedList<BlogPost> posts = await context.BlogPosts
                                                            .Where(b => b.IsDeleted == true)
                                                            .Include(b => b.Category)
                                                            .OrderByDescending(bp => bp.Created)
                                                            .ToPagedListAsync(page, pageSize);

            return posts;
        }

        public async Task<IEnumerable<BlogPost>> GetTopBlogPostsAsync(int numberOfPopular)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            IEnumerable<BlogPost> posts = await context.BlogPosts
                                                       .Where(bp => bp.IsPublished == true && !bp.IsDeleted)
                                                       .Include(bp => bp.Comments)
                                                       .OrderByDescending(bp => bp.Comments.Count())
                                                       .Take(numberOfPopular)
                                                       .ToListAsync();

            return posts;
        }
        #endregion




        #region Get Individual Object
        public async Task<BlogPost?> GetAnyBlogPostByIdAsync(int blogPostId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts
                                    .Include(bp => bp.Tags)
                                    .FirstOrDefaultAsync(bp => bp.Id == blogPostId);

            return blogPost;
        }
        public async Task<BlogPost?> GetPublishedBlogPostBySlugAsync(string slug)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();
            BlogPost? blogPost = await context.BlogPosts
                                    .Where(bp => bp.IsPublished == true && bp.IsDeleted == false)
                                    .Include(bp => bp.Category)
                                    .Include(bp => bp.Tags)
                                    .Include(bp => bp.Comments)
                                        .ThenInclude(c => c.Author)
                                    .FirstOrDefaultAsync(bp => bp.Slug == slug);

            return blogPost;
        }
        #endregion




        #region Change DataBase Item
        public async Task UpdateBlogPostAsync(BlogPost blogPost)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            bool shouldUpdate = await context.BlogPosts.AnyAsync(bp => bp.Id == blogPost.Id);

            if (shouldUpdate)
            {
                ImageUpload? oldImage = null;

                if (blogPost.Image is not null)
                {
                    //save the new image
                    context.Images.Add(blogPost.Image);

                    //check for old image
                    oldImage = await context.Images.FirstOrDefaultAsync(i => i.Id == blogPost.ImageId);

                    //fix foreign key
                    blogPost.ImageId = blogPost.Image.Id;
                }

                blogPost.Slug = await GenerateSlugAsync(blogPost.Title!, blogPost.Id);

                context.BlogPosts.Update(blogPost);
                await context.SaveChangesAsync();

                if (oldImage is not null)
                {
                    context.Images.Remove(oldImage);
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task DeleteBlogPostAsync(int blogPostId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.FirstOrDefaultAsync(bp => bp.Id == blogPostId);

            if (blogPost != null)
            {
                blogPost.IsPublished = false;
                blogPost.IsDeleted = true;

                context.BlogPosts.Update(blogPost);
                await context.SaveChangesAsync();
            }
        }
        public async Task RestoreDeletedBlogPostAsync(int blogPostId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.FirstOrDefaultAsync(bp => bp.Id == blogPostId);

            if (blogPost != null)
            {
                blogPost.IsPublished = false;
                blogPost.IsDeleted = false;

                context.BlogPosts.Update(blogPost);
                await context.SaveChangesAsync();
            }
        }
        public async Task UnpublishBlogPostAsync(int blogPostId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.FirstOrDefaultAsync(bp => bp.Id == blogPostId);

            if (blogPost != null)
            {
                blogPost.IsPublished = false;

                context.BlogPosts.Update(blogPost);
                await context.SaveChangesAsync();
            }
        }
        #endregion



        #region Tags
        public async Task AddTagsToBlogPostAsync(int blogPostId, IEnumerable<string> tagNames)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            TextInfo textInfo = new CultureInfo("en-US").TextInfo;

            BlogPost? blogPost = await context.BlogPosts
                                    .Include(bp => bp.Tags)
                                    .FirstOrDefaultAsync(bp => bp.Id == blogPostId);

            if (blogPost is not null)
            {
                foreach(string tagName in tagNames)
                {
                    Tag? existingTag = await context.Tags.FirstOrDefaultAsync(t => t.Name!.Trim().ToLower() == tagName.Trim().ToLower());

                    if (existingTag is not null) 
                    { 
                        blogPost.Tags.Add(existingTag);
                    }
                    else
                    {
                        string titleCaseTagName = textInfo.ToTitleCase(tagName.Trim());
                        Tag newTag = new Tag() { Name = titleCaseTagName };

                        context.Tags.Add(newTag);
                        blogPost.Tags.Add(newTag);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveTagsFromBlogPostAsync(int blogPostId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts
                                                .Include(bp => bp.Tags)
                                                .FirstOrDefaultAsync(bp => bp.Id == blogPostId);

            if (blogPost is not null)
            {
                blogPost.Tags.Clear();
                await context.SaveChangesAsync();

                //TODO: remove any tags that have no posts???
            }
        }

        public Task PublishBlogPostAsync(int blogPostId)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Slugify
        private async Task<string> GenerateSlugAsync(string title, int id)
        {
            // Somehow tate a title and make a slug? eg "My first post" => "my-first-post"
            //validate tje slug? eg is it already taken in another post?

            //1: generate a slug
            if (await ValidateSlugAsync(title, id))
            {
                return Slugify(title);
            }
            else
            {
                int i = 2;
                string newTitle = $"{title} {i}";
                bool isValid = await ValidateSlugAsync(newTitle, id);

                while (isValid == false)
                {
                    i++;
                    newTitle = $"{title} {i}";
                    isValid = await ValidateSlugAsync(newTitle, id);
                }

                return Slugify(newTitle);
            }
        }

        private async Task<bool> ValidateSlugAsync(string title, int blogId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            string newSlug = Slugify(title);


            bool isValid = false;
            if (blogId == 0)
            {
                // this is a new post, so check if anyone has the same slug
                isValid = !await context.BlogPosts.AnyAsync(bp => bp.Slug == newSlug);
            }
            else
            {
                // this is an existing post. so see if any other posts hace this slug
                isValid = !await context.BlogPosts.AnyAsync(bp => bp.Slug == newSlug && bp.Id != blogId);
            }

            return isValid;
        }

        private static string Slugify(string title)
        {
            if (string.IsNullOrEmpty(title)) return title;

            title = title.Normalize(System.Text.NormalizationForm.FormD);
            char[] chars = title.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

            string normalizedTitle = new string(chars).Normalize(System.Text.NormalizationForm.FormC)
                                                                        .ToLower()
                                                                        .Trim();

            string titleWithoutSymbols = Regex.Replace(normalizedTitle, @"[^A-Za-z0-9\s-]", "");
            string slug = Regex.Replace(titleWithoutSymbols, @"\s+", "-");

            return slug;

        }

        #endregion


    }
}
