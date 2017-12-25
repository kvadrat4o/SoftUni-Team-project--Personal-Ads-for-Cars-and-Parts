namespace Store.Web.Models.ProductViewModels
{
    using AutoMapper;
    using Data.Models.Enums;
    using Store.Data.Models;
    using Store.Infrastructure.Mapping.Interfaces;
    using System.ComponentModel.DataAnnotations;

    using static Data.ModelConstants;

    public class DetailsProductViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string SellerId { get; set; }

        [Required]
        [MinLength(ProductTitleMinLength)]
        [MaxLength(ProductTitleMaxLength)]
        public string Title { get; set; }

        [MaxLength(ProductPicturePathMaxLength)]
        public string PicturePath { get; set; }

        [MinLength(ProductPartNumberMinLength)]
        [MaxLength(ProductPartNumberMaxLength)]
        public string PartNumber { get; set; }

        [MaxLength(ProductDescriptionMaxLength)]
        public string Description { get; set; }

        [Range(MoneyMinValue, MoneyMaxValue)]
        public decimal Price { get; set; }

        [Range(ProductQuantityMinValue, ProductQuantityMaxValue)]
        public int Quantity { get; set; }

        public Category Category { get; set; }
    }
}
