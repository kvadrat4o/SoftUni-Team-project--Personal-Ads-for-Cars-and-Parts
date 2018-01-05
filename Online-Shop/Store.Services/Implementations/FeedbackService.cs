using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Models;
using Store.Services.Interfaces;
using Store.Services.Models.FeedbackViewModels;
using System;
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

        public async Task<Feedback> EditFeedback(DetailsFeedbackViewModel feedback, string loggedUserId)
        {
            var feedbackToEdit = await this.db.Feedbacks.Where(f => f.SenderId == feedback.SenderId && f.ProductId == feedback.ProductId).FirstOrDefaultAsync();
            if (feedbackToEdit == null || !feedback.SenderId.Equals(loggedUserId))
            {
                throw new InvalidOperationException( "You are not allowed to edit someone else's feedbacks");
            }

            feedbackToEdit.Content = feedback.Content;
            feedbackToEdit.Rating = feedback.Rating;

            await this.db.SaveChangesAsync();

            return feedbackToEdit;
        }

        public void Delete(Feedback feedback)
        {
            this.db.Feedbacks.Remove(feedback);
            this.db.SaveChanges();
        }

        public async Task<TModel> GetFeedbackAsync<TModel>(int productId, string senderId) => await this.db.Feedbacks
            .Where(f => f.ProductId == productId && f.SenderId == senderId)
            .ProjectTo<TModel>()
            .FirstOrDefaultAsync();

        public async Task<Feedback> GetFeedbackAsync(int productId, string senderId) => await this.db.Feedbacks
            .Where(f => f.ProductId == productId && f.SenderId == senderId)
            .FirstOrDefaultAsync();

        public ListFeedbackProductViewModel[] GetUserFeedbacks(string senderId) => this.db.Feedbacks.
            Where(f => f.SenderId == senderId)
            .ProjectTo<ListFeedbackProductViewModel>()
            .ToArray();

        public ListFeedbackProductViewModel[] GetUserReceivedFeedbacks(string receiverId) => this.db.Feedbacks.
            Where(f => f.ReceiverId == receiverId)
            .ProjectTo<ListFeedbackProductViewModel>()
            .ToArray();

        public ListFeedbackProductViewModel[] GetProductFeedbacks(int productId) => this.db.Feedbacks
            .Where(f => f.ProductId == productId)
            .ProjectTo<ListFeedbackProductViewModel>()
            .ToArray();

    }
}
