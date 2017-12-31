namespace Store.Web.Models.ProductViewModels
{
    using Store.Data.Models;
    using Store.Data.Models.Enums;
    using Store.Helpers.Interfaces.Mapping;

    public class CatalogProductViewModel: IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PartNumber { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Category Category { get; set; }

    }
}
