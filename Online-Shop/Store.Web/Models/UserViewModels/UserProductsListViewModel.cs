namespace Store.Web.Models.UserViewModels
{
    using Store.Web.Models.ProductViewModels;
    using System.Collections.Generic;

    public class UserProductsListViewModel
    {
        public string UserName { get; set; }

        public ICollection<ProductDetailsViewModel> ProductsToSell { get; set; }
    }
}
