namespace Store.Web.Models.UserViewModels
{
    using Store.Web.Models.ProductViewModels;
    using System.Collections.Generic;

    public class UserProductsListViewModel
    {
        public int Id { get; set; }

        public string SellerUserName { get; set; }

        public ICollection<DetailsProductViewModel> ProductsToSell { get; set; }
    }
}
