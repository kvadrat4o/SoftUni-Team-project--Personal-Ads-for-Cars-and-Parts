using Store.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<string> CreateAsync(Feedback feedback);

        void Delete(Feedback feedback);

        Task<TModel> GetFeedbackAsync<TModel>(int productId, string senderId);
    }
}
