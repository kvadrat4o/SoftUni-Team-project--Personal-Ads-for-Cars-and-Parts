namespace Store.Services.Interfaces
{
    using Store.Data.Models;
    using Store.Services.Models.FeedbackViewModels;
    using System.Threading.Tasks;

    public interface IFeedbackService
    {
        Task<string> CreateAsync(Feedback feedback);

        void Delete(Feedback feedback);

        Task<TModel> GetFeedbackAsync<TModel>(int productId, string senderId);

        ListFeedbackProductViewModel[] GetUserFeedbacks(string senderId);
    }
}
