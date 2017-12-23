namespace Store.Services.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using System;
    using System.Threading.Tasks;
    using Store.Services.Models.ProductViewModels;

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

        public async Task<string> CreateAsync(Product product)
        {
            var existingProduct = await this.db.Products
                .FirstOrDefaultAsync(p => p.Title.Equals(product.Title, StringComparison.OrdinalIgnoreCase));
            if (existingProduct != null)
            {
                return $"There is already a product with that title: {product.Title}";
            }

            var isTaxPayed = await this.TryPayTaxAsync(product.SellerId);
            if (!isTaxPayed)
            {
                return $"You don't have enough money to publish a product! Please charge your account.";
            }

            this.db.Products.Add(product);
            this.db.SaveChanges();

            return null;
        }

        public void Delete(Product product)
        {
            db.Products.Remove(product);
            db.SaveChanges();
        }

        public async Task<Product> GetProduct(string title)
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Title.Equals(title));
            return product;
        }

        private async Task<bool> TryPayTaxAsync(string sellerId)
        {
            var seller = await this.db.Users.FindAsync(sellerId);
            var sellerRoles = await this.userManager.GetRolesAsync(seller);

            var isSellerAdmin = sellerRoles.Contains(ModelConstants.AdminRoleName);

            if (isSellerAdmin)
            {
                return true;
            }

            if (seller.MoneyBalance < ServiceConstants.ProductListingPriceTax)
            {
                return false;
            }

            seller.MoneyBalance -= ServiceConstants.ProductListingPriceTax;
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<Product> Edit(string oldProductTitle, EditProductViewModel newProductData)
        {
            var productToEdit = await this.GetProduct(oldProductTitle);

            if (newProductData.PicturePath != null)
            {
                productToEdit.PicturePath = newProductData.PicturePath;
            }
            if (newProductData.PartNumber != null)
            {
                productToEdit.PartNumber = newProductData.PartNumber;
            }
            if (newProductData.Description != null)
            {
                productToEdit.Description = newProductData.Description;
            }

            productToEdit.Price = newProductData.Price;
            productToEdit.Quantity = newProductData.Quantity;

            await db.SaveChangesAsync();

            return productToEdit;
        }
    }
}
