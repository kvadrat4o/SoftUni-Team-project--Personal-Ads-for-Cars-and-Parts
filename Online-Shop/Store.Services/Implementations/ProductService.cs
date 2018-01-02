﻿namespace Store.Services.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using System;
    using System.Threading.Tasks;
    using Store.Services.Models.ProductViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using Store.Data.Models.Enums;
    using AutoMapper.QueryableExtensions;

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
                return $"There is already a product with this title: {product.Title}";
            }

            var isTaxPayed = await this.TryPayListingTaxAsync(product.SellerId);
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

        public async Task<TModel> GetProductAsync<TModel>(int id, string sellerId) => await db.Products
            .Where(p => p.Id == id && p.SellerId == sellerId)
            .ProjectTo<TModel>()
            .FirstOrDefaultAsync();

        public async Task<TModel> GetProductAsync<TModel>(int id) => await db.Products
            .Where(p => p.Id == id)
            .ProjectTo<TModel>()
            .FirstOrDefaultAsync();

        public async Task<Product> GetProductAsync(int id) => await db.Products
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        public async Task<Product> GetProductAsync(int id, string sellerId) => await db.Products
            .Where(p => p.Id == id && p.SellerId == sellerId)
            .FirstOrDefaultAsync();

        public async Task<Product> GetProductAsync(string title) => await db.Products.FirstOrDefaultAsync(p => p.Title.Equals(title));

        private async Task<bool> TryPayListingTaxAsync(string sellerId)
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

        public async Task<Product> Edit(EditProductViewModel newProductData, string requestUserId)
        {
            var productToEdit = await this.db.FindAsync<Product>(newProductData.Id);
            if (requestUserId != productToEdit.SellerId)
            {
                throw new InvalidOperationException("You are not allowed to edit a product which is not created by you!");
            }

            await this.SetChangedValuesAsync(productToEdit, newProductData);
            await db.SaveChangesAsync();

            return productToEdit;
        }

        public ProductDetailsViewModel[] ProductsBySeller(string sellerId) => this.db.Products
            .Where(p => p.SellerId == sellerId && p.IsActive)
            .ProjectTo<ProductDetailsViewModel>()
            .ToArray();

        public List<Product> AllProductsForSale() => this.db.Products.ToList();

        private async Task SetChangedValuesAsync(Product productToEdit, EditProductViewModel newProductData)
        {
            if (!newProductData.Title.Equals(productToEdit.Title, StringComparison.OrdinalIgnoreCase))
            {
                if (await this.db.Products
                    .AnyAsync(p => p.Title.Equals(newProductData.Title, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Already there is a product named: {newProductData.Title}");
                }

                productToEdit.Title = newProductData.Title;
            }

            if (newProductData.PicturePath != productToEdit.PicturePath)
            {
                productToEdit.PicturePath = newProductData.PicturePath;
            }

            if (newProductData.PartNumber != productToEdit.PartNumber)
            {
                productToEdit.PartNumber = newProductData.PartNumber;
            }

            if (newProductData.Description != null)
            {
                productToEdit.Description = newProductData.Description;
            }

            if (newProductData.Price != productToEdit.Price)
            {
                productToEdit.Price = newProductData.Price;
            }

            if (newProductData.Quantity != productToEdit.Quantity)
            {
                productToEdit.Quantity = newProductData.Quantity;
            }

            if (newProductData.Category != productToEdit.Category)
            {
                productToEdit.Category = newProductData.Category;
            }
        }

        public List<TModel> ProductsByCategory<TModel>(Category category) => this.db.Products
            .Where(p => p.Category.Equals(category))
            .ProjectTo<TModel>()
            .ToList();
    }
}
