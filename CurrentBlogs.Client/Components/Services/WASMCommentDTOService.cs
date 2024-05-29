using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Pages.AuthorMenu.BlogPosts;
using CurrentBlogs.Client.Components.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace CurrentBlogs.Client.Components.Services
{
    public class WASMCommentDTOService : ICommentsDTOService
    {

        private readonly HttpClient _httpClient;

        public WASMCommentDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Get List of items
        public async Task<IEnumerable<CommentDTO>> GetCommentsByBlogPostIdAsync(int blogPostId)
        {
            IEnumerable<CommentDTO> comments = await _httpClient.GetFromJsonAsync<IEnumerable<CommentDTO>>($"api/comments/updated/{blogPostId}") ?? [];
            return comments;
        }
        #endregion

        #region Get One Item
        //Not yet used in my code
        public async Task<CommentDTO?> GetCommentByIdAsync(int commentId)
        {
            CommentDTO? comment = await _httpClient.GetFromJsonAsync<CommentDTO>($"api/comments/{commentId}");
            return comment;
        }
        #endregion

        #region Update Item in DB
        public async Task CreateCommentAsync(CommentDTO comment, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/comments/create", comment);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteCommentAsync(int commentId)
        {
            HttpResponseMessage? response = await _httpClient.DeleteAsync($"api/comments/{commentId}");
        }
        public async Task UpdateCommentAsync(CommentDTO comment, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<CommentDTO>($"api/comments/{comment.Id}", comment);
            response.EnsureSuccessStatusCode();
        }
        #endregion




    }
}
