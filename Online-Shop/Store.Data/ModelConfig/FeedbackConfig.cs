namespace Store.Data.ModelConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Store.Data.Models;

    public class FeedbackConfig : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder
                .HasAlternateKey(e => new { e.ProductId, e.SenderId });

            builder
                .HasOne(e => e.Sender)
                .WithMany(u => u.SentFeedbacks)
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Receiver)
                .WithMany(u => u.ReceivedFeedbacks)
                .HasForeignKey(e => e.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
