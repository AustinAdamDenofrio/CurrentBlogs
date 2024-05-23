using CurrentBlogs.Client.Components.Models;

namespace CurrentBlogs.Client.Components.Services.Interfaces
{
    public interface ICategoryDTOService
    {
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task UpdateCategoryAsync(CategoryDTO category);
        Task DeleteCategoryAsync(int categoryId);
        Task<IEnumerable<CategoryDTO>> GetTopCategoriesAsync(int quantityOfTop);
    }
}
