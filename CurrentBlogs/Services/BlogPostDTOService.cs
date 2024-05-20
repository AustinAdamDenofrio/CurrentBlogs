using CurrentBlogs.Client.Components.Models;
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
    }
}

