namespace Store.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Store.Data.ModelConfig;
    using Store.Data.Models;

    public class StoreDbContext : IdentityDbContext<User>
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductInvoice> ProductsInvoices { get; set; }

        public DbSet<ShippingRecord> ShippingRecords { get; set; }

        public DbSet<Town> Towns { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CarConfig());
            builder.ApplyConfiguration(new CountryConfig());
            builder.ApplyConfiguration(new FeedbackConfig());
            builder.ApplyConfiguration(new InvoiceConfig());
            builder.ApplyConfiguration(new ProductConfig());
            builder.ApplyConfiguration(new ProductInvoiceConfig());
            builder.ApplyConfiguration(new TownConfig());
            builder.ApplyConfiguration(new UserConfig());
        }
    }
}
