using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Pages.AuthorMenu.BlogPosts;
using CurrentBlogs.Client.Components.Services.Interfaces;
using CurrentBlogs.Helper;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using NuGet.Protocol.Core.Types;

namespace CurrentBlogs.Services
{
    public class BlogPostDTOService : IBlogPostDTOService
    {
        private readonly IBlogPostsRepository _repository;
        public BlogPostDTOService(IBlogPostsRepository repository) 
        { 
            _repository = repository;
        }
        public async Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDTO)
        {
            BlogPost newPost = new BlogPost()
            {
                Title = blogPostDTO.Title,
                Abstract = blogPostDTO.Abstract,
                Content = blogPostDTO.Content,
                IsPublished = blogPostDTO.IsPublished,
                IsDeleted = false,
                CategoryId = blogPostDTO.CategoryId,
                Created = DateTimeOffset.Now,
            };

            if (blogPostDTO.ImageUrl?.StartsWith("data") == true)
            {
                try
                {
                    newPost.Image = UploadHelper.GetImageUpload(blogPostDTO.ImageUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            //TODO: tags???
            newPost = await _repository.CreateBlogPostAsync(newPost);
            return newPost.ToDTO();
        }

        public async Task DeleteBlogPostAsync(int blogPostId)
        {
            await _repository.DeleteBlogPostAsync(blogPostId);
        }

        public async Task<IEnumerable<BlogPostDTO>> GetAllBlogPostsAsync()
        {
            IEnumerable<BlogPost> createPosts = await _repository.GetAllBlogPostsAsync();

            IEnumerable<BlogPostDTO> postsDTO = createPosts.Select(bp => bp.ToDTO());

            return postsDTO;
        }

        public async Task<BlogPostDTO?> GetBlogPostByIdAsync(int blogPostId)
        {
            BlogPost? blogPost = await _repository.GetBlogPostByIdAsync(blogPostId);
            return blogPost?.ToDTO();
        }

        public async Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO)
        {
            //get post from db that will be updated to a new value from the parameter
            BlogPost? blogPostToUpdate = await _repository.GetBlogPostByIdAsync(blogPostDTO.Id);

            if (blogPostToUpdate is not null)
            {
                blogPostToUpdate.Title = blogPostDTO.Title;
                blogPostToUpdate.Abstract = blogPostDTO.Abstract;
                blogPostToUpdate.Content = blogPostDTO.Content;
                blogPostToUpdate.IsPublished = blogPostDTO.IsPublished;
                blogPostToUpdate.CategoryId = blogPostDTO.CategoryId;
                blogPostToUpdate.Updated = DateTimeOffset.Now;
                blogPostToUpdate.IsDeleted = blogPostDTO.IsDeleted;

                if (blogPostDTO.ImageUrl?.StartsWith("data:") == true)
                {
                    blogPostToUpdate.Image = UploadHelper.GetImageUpload(blogPostDTO.ImageUrl);
                }
                else
                {
                    blogPostToUpdate.Image = null;
                }

                // categoryToUpdate.Contacts.Clear();

                await _repository.UpdateBlogPostAsync(blogPostToUpdate);
            }
        }
    }
}

