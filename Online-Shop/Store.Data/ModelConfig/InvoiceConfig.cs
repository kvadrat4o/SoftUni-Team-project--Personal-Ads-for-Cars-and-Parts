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
                .Property(e => e.IssueDate)
                .HasDefaultValueSql("GETDATE()");

            builder
                .HasOne(e => e.Seller)
                .WithMany(u => u.SoldInvoices)
                .HasForeignKey(e => e.SellerId);

            builder
                .HasOne(e => e.Buyer)
                .WithMany(u => u.BoughtInvoices)
                .HasForeignKey(e => e.BuyerId);
        }
    }
}
