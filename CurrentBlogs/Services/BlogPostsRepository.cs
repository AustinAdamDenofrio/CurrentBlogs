using CurrentBlogs.Data;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
                    newTitle = $"{title} {i}"; //my-first-blog-post-2
                    isValid = await ValidateSlugAsync(newTitle, id);
                }

                return Slugify(newTitle);
            }
            //2: in not valid make a new one
            //3: return a valid one
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
                isValid = !await context.BlogPosts.AnyAsync(bp => bp.Slug == newSlug && bp.Id == blogId);
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
    }
}
