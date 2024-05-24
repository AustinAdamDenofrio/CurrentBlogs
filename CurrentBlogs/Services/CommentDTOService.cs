using CurrentBlogs.Client.Components.Models;
using CurrentBlogs.Client.Components.Services.Interfaces;
using CurrentBlogs.Helper;
using CurrentBlogs.Models;
using CurrentBlogs.Services.Interfaces;
using NuGet.Protocol.Core.Types;

namespace CurrentBlogs.Services
{
    public class CommentDTOService : ICommentsDTOService
    {
        private readonly ICommentsRepository _repository;
        public CommentDTOService(ICommentsRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateCommentAsync(CommentDTO comment, string UserId)
        {
            Comment newComment = new Comment()
            {
                Content = comment.Content,
                AuthorId = UserId,
                BlogPostId = comment.BlogPostId,
            };

            newComment = await _repository.CreateCommentAsync(newComment);
        }

        public async Task UpdateCommentAsync(CommentDTO comment, string UserId)
        {
            Contact? contact = await _repository.GetContactByIdAsync(contactDTO.Id, UserId);

            if (contact is not null)
            {
                contact.FirstName = contactDTO.FirstName;
                contact.LastName = contactDTO.LastName;
                contact.BirthDate = contactDTO.BirthDate;
                contact.Address = contactDTO.Address;
                contact.Address2 = contactDTO.Address2;
                contact.City = contactDTO.City;
                contact.State = contactDTO.State;
                contact.ZipCode = contactDTO.ZipCode;
                contact.Email = contactDTO.Email;
                contact.PhoneNumber = contactDTO.PhoneNumber;

                if (contactDTO.ImageUrl?.StartsWith("data:") == true)
                {
                    contact.Image = UploadHelper.GetImageUpload(contactDTO.ImageUrl);
                }
                else
                {
                    contact.Image = null;
                }

                // dont let db update cats yet
                contact.Categories.Clear();
                await repository.UpdateContactAsync(contact);

                //remove all the old cats
                await repository.RemoveCategoriesFromContactAsync(contact.Id, userId);

                //add back the cats based on the users selected 
                IEnumerable<int> selectedCategoryIds = contactDTO.Categories.Select(c => c.Id);
                await repository.AddCategoriesToContactAsync(contact.Id, userId, selectedCategoryIds);
            }
        }
    }
}
