namespace Store.Services.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;

    public class ProductService : IProductService
    {
        private StoreDbContext db;
        private UserManager<User> userManager;

        public ProductService(StoreDbContext db,
            UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public void Create(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
        }
    }
}
