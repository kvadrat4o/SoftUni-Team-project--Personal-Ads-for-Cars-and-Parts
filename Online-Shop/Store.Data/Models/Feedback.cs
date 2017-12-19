namespace Store.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static ModelConstants;

    public class Feedback
    {
        public int Id { get; set; }

        [Required]
        [MinLength(FeedbackContentMinLength)]
        [MaxLength(FeedbackContentMaxLength)]
        public string Content { get; set; }

        [Range(FeedbackRatingMinValue, FeedbackRatingMaxValue)]
        public byte Rating { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}