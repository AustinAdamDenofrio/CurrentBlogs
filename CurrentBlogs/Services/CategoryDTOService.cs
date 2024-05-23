using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Services.Interfaces;
using CurrentBlogs.Helper;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using NuGet.Protocol.Core.Types;

namespace CurrentBlogs.Services
{
    public class CategoryDTOService(ICategoryRepository repository, IBlogPostDTOService BlogPostService) : ICategoryDTOService
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

            IEnumerable<CategoryDTO> categoriesDTO = createCategories.Select(c => c.ToDTO());
            
            return categoriesDTO;
        }

        //Validate the AppUser has permission to delete
        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            Category? category = await repository.GetCategoryByIdAsync(id);
            return category?.ToDTO();
        }

        public async Task<IEnumerable<CategoryDTO>> GetTopCategoriesAsync(int quantityOfTop)
        {
            IEnumerable<Category> createTopCategories = await repository.GetTopCategoriesAsync(quantityOfTop);

            IEnumerable<CategoryDTO> categoriesTopDTO = createTopCategories.Select(c => c.ToDTO());

            return categoriesTopDTO;
        }


        //Validate the AppUser has permission to UpdateCategory
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
    }
}
