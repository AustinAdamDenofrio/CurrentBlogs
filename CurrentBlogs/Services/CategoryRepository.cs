using CurrentBlogs.Data;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrentBlogs.Services
{
    public class CategoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ICategoryRepository
    {
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return category;
        }

        //Validate the AppUser has permission to delete
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

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Category> categories = await context.Categories
                                                            .Include(c => c.BlogPosts)
                                                            .ToListAsync();

            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Category? category = await context.Categories
                                    .FirstOrDefaultAsync(c => c.Id == id);

            return category;
        }

        //Validate the AppUser has permission to UpdateCategory
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
    }
}
