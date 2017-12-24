namespace Store.Services.Implementations
{
    using Microsoft.AspNetCore.Http;
    using Store.Services.Interfaces;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ImageService : IImageService
    {
        public readonly string[] AllowedImageTypes = new[] { "jpeg", "jpg", "png" };

        public async Task<string> SaveImageAsync(IFormFile image, string username, string imageName)
        {
            var extensionStartsAt = image.FileName.LastIndexOf('.');

            if (image == null || image.Length <= 0 || extensionStartsAt < 0 || 
                username == null || imageName == null)
            {
                return null;
            }

            var directory = this.GetUserDirectory(username);
            var extension = image.FileName.Substring(extensionStartsAt);
            this.ValidateImage(extension, image.Length);
            var fileName = $"{imageName}{extension}";

            var filePath = Path.Combine(directory, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            filePath = filePath.Replace(ServiceConstants.PathToRemove, string.Empty);
            return filePath;
        }

        private string GetUserDirectory(string username)
        {
            var dir = string.Format(ServiceConstants.UserDirectory, username);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        private void ValidateImage(string extension, long length)
        {
            extension = extension.Trim('.').ToLower();
            if (!AllowedImageTypes.Any(e => e.Equals(extension)))
            {
                throw new InvalidOperationException($"Allowed file types: {string.Join(", ", AllowedImageTypes)}");
            }

            if (length > ServiceConstants.ImageMaxSizeInBytes)
            {
                throw new InvalidOperationException($"Allowed file size is up to: {ServiceConstants.ImageMaxSizeInMB}MB");
            }
        }
    }
}
