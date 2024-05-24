using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Pages.AuthorMenu.BlogPosts;
using CurrentBlogs.Client.Components.Services.Interfaces;
using CurrentBlogs.Helper;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol.Core.Types;
using System.Drawing.Printing;

namespace CurrentBlogs.Services
{
    public class BlogPostDTOService : IBlogPostDTOService
    {
        private readonly IBlogPostsRepository _repository;
        public BlogPostDTOService(IBlogPostsRepository repository) 
        { 
            _repository = repository;
        }




        #region Get List of Items
        public async Task<PagedList<BlogPostDTO>> GetPublishedBlogPostsAsync(int page, int pageSize)
        {
            PagedList<BlogPost> posts = await _repository.GetPublishedBlogPostsAsync(page, pageSize);

            PagedList<BlogPostDTO> postsDTO = new();

            postsDTO.TotalPages = posts.TotalPages;
            postsDTO.TotalItems = posts.TotalItems;
            postsDTO.Page = posts.Page;

            List<BlogPostDTO> dtos = new List<BlogPostDTO>();

            foreach (BlogPost blogPost in posts.Data) 
            {
                dtos.Add(blogPost.ToDTO());
            }

            postsDTO.Data = dtos;

            return postsDTO;
        }
        public async Task<PagedList<BlogPostDTO>> GetDraftPostsAsync(int page, int pageSize)
        {
            PagedList<BlogPost> posts = await _repository.GetDraftPostsAsync(page, pageSize);

            PagedList<BlogPostDTO> postsDTO = new();

            postsDTO.TotalPages = posts.TotalPages;
            postsDTO.TotalItems = posts.TotalItems;
            postsDTO.Page = posts.Page;

            List<BlogPostDTO> dtos = new List<BlogPostDTO>();

            foreach (BlogPost blogPost in posts.Data)
            {
                dtos.Add(blogPost.ToDTO());
            }

            postsDTO.Data = dtos;

            return postsDTO;
        }
        public async Task<PagedList<BlogPostDTO>> GetDeletedPostsAsync(int page, int pageSize)
        {
            PagedList<BlogPost> posts = await _repository.GetDeletedPostsAsync(page, pageSize);

            PagedList<BlogPostDTO> postsDTO = new();

            postsDTO.TotalPages = posts.TotalPages;
            postsDTO.TotalItems = posts.TotalItems;
            postsDTO.Page = posts.Page;

            List<BlogPostDTO> dtos = new List<BlogPostDTO>();

            foreach (BlogPost blogPost in posts.Data)
            {
                dtos.Add(blogPost.ToDTO());
            }

            postsDTO.Data = dtos;

            return postsDTO;
        }

        public async Task<IEnumerable<BlogPostDTO>> GetTopBlogPostsAsync(int numberOfPopular)
        {
            IEnumerable<BlogPost> createTopPosts = await _repository.GetTopBlogPostsAsync(numberOfPopular);

            IEnumerable<BlogPostDTO> topPostsDTO = createTopPosts.Select(bp => bp.ToDTO());

            return topPostsDTO;
        }
        #endregion





        #region Get Individual Item
        public async Task<BlogPostDTO?> GetAnyBlogPostByIdAsync(int blogPostId)
        {
            BlogPost? blogPost = await _repository.GetAnyBlogPostByIdAsync(blogPostId);
            return blogPost?.ToDTO();
        }
        public async Task<BlogPostDTO?> GetPublishedBlogPostBySlugAsync(string slug)
        {
            BlogPost? blogPost = await _repository.GetPublishedBlogPostBySlugAsync(slug);
            return blogPost?.ToDTO();
        }
        #endregion




        #region Update DB Item
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
            newPost = await _repository.CreateBlogPostAsync(newPost);

            IEnumerable<string> tagNames = blogPostDTO.Tags.Select(t => t.Name!);
            await _repository.AddTagsToBlogPostAsync(newPost.Id, tagNames);

            return newPost.ToDTO();
        }
        public async Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO)
        {
            //remove tags from blogpost

            //get post from db that will be updated to a new value from the parameter
            BlogPost? blogPostToUpdate = await _repository.GetAnyBlogPostByIdAsync(blogPostDTO.Id);

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

                blogPostToUpdate.Tags.Clear();

                IEnumerable<string> tagNames = blogPostDTO.Tags.Select(t => t.Name!);
                await _repository.AddTagsToBlogPostAsync(blogPostToUpdate.Id, tagNames);

                await _repository.UpdateBlogPostAsync(blogPostToUpdate);
            }
        }
        public async Task PublishBlogPostAsync(int blogPostId)
        {
            await _repository.PublishBlogPostAsync(blogPostId);
        }
        public async Task UnpublishBlogPostAsync(int blogPostId)
        {
            await _repository.UnpublishBlogPostAsync(blogPostId);
        }
        public async Task DeleteBlogPostAsync(int blogPostId)
        {
            await _repository.DeleteBlogPostAsync(blogPostId);
        }
        public async Task RestoreDeletedBlogPostAsync(int blogPostId)
        {
            await _repository.RestoreDeletedBlogPostAsync(blogPostId);
        }
        #endregion












    }
}

