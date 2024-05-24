using CurrentBlogs.Client.Components.Models;

namespace CurrentBlogs.Client.Components.Services.Interfaces
{
    public interface ICategoryDTOService
    {
        #region Get Item List
        Task<PagedList<CategoryDTO>> GetAllCategoriesAsync(int page, int pageSize);
        Task<IEnumerable<CategoryDTO>> GetTopCategoriesAsync(int quantityOfTop);
        #endregion



        #region Get Item
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        #endregion



        #region Update DB Items
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);
        Task UpdateCategoryAsync(CategoryDTO category);
        Task DeleteCategoryAsync(int categoryId);
        #endregion


    }
}
