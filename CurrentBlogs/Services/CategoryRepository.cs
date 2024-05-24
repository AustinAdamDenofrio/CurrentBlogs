using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Data;
using CurrentBlogs.Helper.Extensions;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrentBlogs.Services
{
    public class CategoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ICategoryRepository
    {

        #region Get Item List
        public async Task<PagedList<Category>> GetAllCategoriesAsync(int page, int pageSize)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            PagedList<Category> categories = await context.Categories
                                                            .Include(c => c.BlogPosts)
                                                            .OrderBy(bp => bp.Name)
                                                            .ToPagedListAsync(page, pageSize);

            return categories;
        }
        public async Task<IEnumerable<Category>> GetTopCategoriesAsync(int quantityOfTop)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();


            IEnumerable<Category> topCategories = await context.Categories
                                    .Include(c => c.BlogPosts.Where(p => p.IsPublished == true && !p.IsDeleted))
                                    .OrderByDescending(c => c.BlogPosts.Count())
                                    .Take(quantityOfTop)
                                    .ToListAsync();

            return topCategories;
        }
        #endregion



        #region Get Item
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Category? category = await context.Categories
                                    .Include (c => c.BlogPosts)
                                    .FirstOrDefaultAsync(c => c.Id == id);

            return category;
        }
        #endregion



        #region Update DB Items
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return category;
        }
        public async Task UpdateCategoryAsync(Category category)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldUpdate = await context.Categories.AnyAsync(c => c.Id == category.Id);

            if (shouldUpdate)
            {
                ImageUpload? oldImage = null;

                if (category.Image is not null)
                {
                    //save the new image
                    context.Images.Add(category.Image);

                    //check for old image
                    oldImage = await context.Images.FirstOrDefaultAsync(i => i.Id == category.ImageId);

                    //fix foreign key
                    category.ImageId = category.Image.Id;
                }

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                if (oldImage is not null)
                {
                    context.Images.Remove(oldImage);
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task DeleteCategoryAsync(int categoryId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category != null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
        }
        #endregion

    }
}
