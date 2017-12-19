namespace Store.Data.ModelConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Store.Data.Models;

    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .HasAlternateKey(e => e.Title);

            builder
                .Ignore(e => e.EndDate);

            builder
                .Property(e => e.StartDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
