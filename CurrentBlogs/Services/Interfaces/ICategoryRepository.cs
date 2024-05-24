using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Models;

namespace CurrentBlogs.Services.Interfaces
{
    public interface ICategoryRepository
    {

        #region Get Item List
        Task<PagedList<Category>> GetAllCategoriesAsync(int page, int pageSize);
        Task<IEnumerable<Category>> GetTopCategoriesAsync(int quantityOfTop);
        #endregion



        #region Get Item
        Task<Category?> GetCategoryByIdAsync(int id);
        #endregion



        #region Update DB Items
        Task<Category> CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int categoryId);

        #endregion
    }
}
