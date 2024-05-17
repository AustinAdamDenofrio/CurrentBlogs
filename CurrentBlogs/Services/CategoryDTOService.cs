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


        //Pass user validation? UserID? Who can delete these?
        public async Task DeleteCategoryAsync(int categoryId)
        {
            await repository.DeleteCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            IEnumerable<Category> createCategories = await repository.GetAllCategoriesAsync();

            IEnumerable<CategoryDTO> categoryDTO = createCategories.Select(c => c.ToDTO());

            return categoryDTO;
        }

        //Validate the AppUser has permission to delete
        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            Category? category = await repository.GetCategoryByIdAsync(id);
            return category?.ToDTO();
        }

        
        //Validate the AppUser has permission to UpdateCategory
        public async Task UpdateCategoryAsync(CategoryDTO categoryDTO)
        {
            Category? category = await repository.GetCategoryByIdAsync(categoryDTO.Id);

            if (category is not null)
            {
                category.Name = categoryDTO.Name;
                category.Description = categoryDTO.Description;
                //Add images and Descriptions
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
    }
}
