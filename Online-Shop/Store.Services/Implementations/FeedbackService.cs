using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Models;
using Store.Services.Interfaces;
using Store.Services.Models.FeedbackViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Services.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private StoreDbContext db;

        public FeedbackService(StoreDbContext db)
        {
            this.db = db;
        }
        public async Task<string> CreateAsync(Feedback feedback)
        {
            var existingProduct = await this.db.Feedbacks.FirstOrDefaultAsync(f => f.SenderId == feedback.SenderId && f.ProductId == feedback.ProductId);

            if (existingProduct != null)
            {
                return "There is already a feedback posted from that sender to this product";
            }

            this.db.Add(feedback);
            this.db.SaveChanges();

            return null;
        }

        public async void Delete(Feedback feedback)
        {
            this.db.Remove(feedback);
            await this.db.SaveChangesAsync();
        }

        public ListFeedbackProductViewModel[] GetUserFeedbacks(string senderId) => this.db.Feedbacks.ProjectTo<ListFeedbackProductViewModel>().ToArray();

        public async Task<TModel> GetFeedbackAsync<TModel>(int productId, string senderId) => await this.db.Feedbacks
            .Where(f => f.ProductId == productId && f.SenderId == senderId)
            .ProjectTo<TModel>()
            .FirstOrDefaultAsync();
    }
}
