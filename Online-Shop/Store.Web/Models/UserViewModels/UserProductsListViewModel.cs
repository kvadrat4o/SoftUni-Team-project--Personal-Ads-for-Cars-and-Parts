namespace Store.Web.Models.UserViewModels
{
    using Store.Web.Models.ProductViewModels;
    using System.Collections.Generic;

    public class UserProductsListViewModel
    {
        public string SellerId { get; set; }

        public string SellerUserName { get; set; }

        public bool IsRequestSenderOwner { get; set; }

        public ICollection<DetailsProductViewModel> ProductsToSell { get; set; }
    }
}
