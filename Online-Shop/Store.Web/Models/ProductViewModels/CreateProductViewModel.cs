namespace Store.Web.Models.ProductViewModels
{
    using Store.Data.Models;
    using Store.Data.Models.Enums;
    using Store.Infrastructure.Mapping.Interfaces;
    using System.ComponentModel.DataAnnotations;

    using static Store.Data.ModelConstants;

    public class CreateProductViewModel : IMapFrom<Product>
    {
        [Required]
        [MinLength(ProductTitleMinLength)]
        [MaxLength(ProductTitleMaxLength)]
        public string Title { get; set; }

        [MaxLength(ProductPicturePathMaxLength)]
        public string PicturePath { get; set; }

        [Range(MoneyMinValue, MoneyMaxValue)]
        public decimal Price { get; set; }

        [MaxLength(ProductDescriptionMaxLength)]
        public string Description { get; set; }

        [MinLength(ProductPartNumberMinLength)]
        [MaxLength(ProductPartNumberMaxLength)]
        public string PartNumber { get; set; }

        public bool IsNew { get; set; }

        [Range(ProductQuantityMinValue, ProductQuantityMaxValue)]
        public int Quantity { get; set; }

        public Category Category { get; set; }
    }
}
