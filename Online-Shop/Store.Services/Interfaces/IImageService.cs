namespace Store.Services.Interfaces
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile image, string username, string imageName);
    }
}
