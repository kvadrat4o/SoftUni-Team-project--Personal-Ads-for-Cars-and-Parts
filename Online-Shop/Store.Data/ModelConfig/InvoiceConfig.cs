namespace Store.Data.ModelConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Store.Data.Models;

    public class InvoiceConfig : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder
                .Ignore(e => e.NetValue);

            builder
                .Ignore(e => e.TotalValue);

            builder
                .Property(e => e.IssueDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
