namespace Store.Services.Interfaces
{
    using Store.Data.Models;
    using Store.Services.Models.FeedbackViewModels;
    using System.Threading.Tasks;

    public interface IFeedbackService
    {
        Task<string> CreateAsync(Feedback feedback);

        Task<Feedback> EditFeedback(DetailsFeedbackViewModel feedback, string loggedUserId);

        void Delete(Feedback feedback);

        Task<TModel> GetFeedbackAsync<TModel>(int productId, string senderId);

        Task<Feedback> GetFeedbackAsync(int productId, string senderId);

        ListFeedbackProductViewModel[] GetProductFeedbacks(int productId);

        ListFeedbackProductViewModel[] GetUserFeedbacks(string senderId);

        ListFeedbackProductViewModel[] GetUserReceivedFeedbacks(string senderId);
    }
}
