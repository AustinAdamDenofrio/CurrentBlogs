using CurrentBlogs.Models;

namespace CurrentBlogs.Services.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int categoryId);
        Task<IEnumerable<Category>> GetTopCategoriesAsync(int quantityOfTop);

    }
}
