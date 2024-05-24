using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Services.Interfaces;
using CurrentBlogs.Helper;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using NuGet.Protocol.Core.Types;

namespace CurrentBlogs.Services
{
    public class CategoryDTOService(ICategoryRepository repository) : ICategoryDTOService
    {

        #region Get Item List
        public async Task<PagedList<CategoryDTO>> GetAllCategoriesAsync(int page, int pageSize)
        {
            PagedList<Category> categories = await repository.GetAllCategoriesAsync(page, pageSize);

            IEnumerable<CategoryDTO> dtos = categories.Data.Select(c => c.ToDTO());

            PagedList<CategoryDTO> categoriesDTO = new PagedList<CategoryDTO>();
            categoriesDTO.Page = categories.Page;
            categoriesDTO.TotalPages = categories.TotalPages;
            categoriesDTO.TotalItems = categories.TotalItems;
            categoriesDTO.Data = dtos;

            return categoriesDTO;
        }
        public async Task<IEnumerable<CategoryDTO>> GetTopCategoriesAsync(int quantityOfTop)
        {
            IEnumerable<Category> createTopCategories = await repository.GetTopCategoriesAsync(quantityOfTop);

            IEnumerable<CategoryDTO> categoriesTopDTO = createTopCategories.Select(c => c.ToDTO());

            return categoriesTopDTO;
        }
        #endregion


        #region Get Item
        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            Category? category = await repository.GetCategoryByIdAsync(id);
            return category?.ToDTO();
        }
        #endregion


        #region Update DB Items
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category)
        {
            Category newCategory = new Category()
            {
                Name = category.Name,
                Description = category.Description,

            };

            if (category.ImageUrl?.StartsWith("data:") == true)
            {
                newCategory.Image = UploadHelper.GetImageUpload(category.ImageUrl);
            }

            Category createdCategory = await repository.CreateCategoryAsync(newCategory);

            return createdCategory.ToDTO();
        }
        public async Task UpdateCategoryAsync(CategoryDTO categoryDTO)
        {
            Category? category = await repository.GetCategoryByIdAsync(categoryDTO.Id);

            if (category is not null)
            {
                category.Name = categoryDTO.Name;
                category.Description = categoryDTO.Description;
                if (categoryDTO.ImageUrl?.StartsWith("data:") == true)
                {
                    category.Image = UploadHelper.GetImageUpload(categoryDTO.ImageUrl);
                }
                else
                {
                    category.Image = null;
                }

                // categoryToUpdate.Contacts.Clear();

                await repository.UpdateCategoryAsync(category);
            }
        }
        public async Task DeleteCategoryAsync(int categoryId)
        {
            await repository.DeleteCategoryAsync(categoryId);
        }
        #endregion
    }
}
