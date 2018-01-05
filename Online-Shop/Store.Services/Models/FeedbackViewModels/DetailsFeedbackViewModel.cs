namespace Store.Services.Models.FeedbackViewModels
{
    using Store.Data.Models;
    using Store.Helpers.Interfaces.Mapping;
    using System.ComponentModel.DataAnnotations;
    using static Store.Data.ModelConstants;

    public class DetailsFeedbackViewModel : IMapFrom<Feedback>
    {
        [Required]
        [MinLength(FeedbackContentMinLength)]
        [MaxLength(FeedbackContentMaxLength)]
        public string Content { get; set; }

        [Range(FeedbackRatingMinValue, FeedbackRatingMaxValue)]
        public byte Rating { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }


    }
}
