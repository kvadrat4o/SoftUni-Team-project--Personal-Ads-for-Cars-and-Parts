namespace Store.Data.ModelConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Store.Data.Models;

    public class ProductInvoiceConfig : IEntityTypeConfiguration<ProductInvoice>
    {
        public void Configure(EntityTypeBuilder<ProductInvoice> builder)
        {
            builder
                .HasKey(e => new { e.ProductId, e.InvoiceId });

            builder
                .HasOne(e => e.Product)
                .WithMany(p => p.ProductInvoices)
                .HasForeignKey(e => e.ProductId);

            builder
                .HasOne(e => e.Invoice)
                .WithMany(i => i.InvoiceProducts)
                .HasForeignKey(e => e.InvoiceId);
        }
    }
}
