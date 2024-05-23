using Microsoft.AspNetCore.Components.Forms;

namespace CurrentBlogs.Client.Components.Helper
{
    public static class ImageHelper
    {
        public static readonly string DefaultCategoryPicture = "/images/Default_Category_Img.jpg";
        public static readonly string DefaultBlogImage = "/images/Default_Category_Img.jpg";
        public static readonly string DefaultContactImage = "/images/DefaultContactPicture.png";
        public static readonly string DefaultProfileImage = "/images/DefaultProfile.png";
        public static readonly int MaxFileSize = 5 * 1024 * 1024;

        public static async Task<string> GetDataUrl(IBrowserFile file)
        {
            using Stream fileStream = file.OpenReadStream(MaxFileSize);
            using MemoryStream ms = new MemoryStream();
            await fileStream.CopyToAsync(ms);

            byte[] imageBytes = ms.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);
            string dataUrl = $"data:{file.ContentType};base64,{base64String}";

            return dataUrl; 
        }
    }
}
