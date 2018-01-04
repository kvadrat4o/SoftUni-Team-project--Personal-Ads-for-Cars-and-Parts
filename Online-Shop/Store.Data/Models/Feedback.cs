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

        [Required]
        public string SenderId { get; set; }

        public User Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public User Receiver { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}