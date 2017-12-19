namespace Store.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static ModelConstants;

    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [MinLength(CommentContentMinLength)]
        [MaxLength(CommentContentMaxLength)]
        public string Content { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime Date { get; set; }
    }
}
