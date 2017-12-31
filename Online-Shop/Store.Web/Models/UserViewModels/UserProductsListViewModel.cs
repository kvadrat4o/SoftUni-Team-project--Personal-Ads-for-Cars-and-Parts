namespace Store.Web.Models.UserViewModels
{
    using Store.Services.Models.ProductViewModels;
    using System.Collections.Generic;

    public class UserProductsListViewModel
    {
        public string SellerId { get; set; }

        public string SellerUserName { get; set; }

        public bool IsRequestSenderOwner { get; set; }

        public ICollection<ProductDetailsViewModel> ProductsToSell { get; set; }
    }
}
