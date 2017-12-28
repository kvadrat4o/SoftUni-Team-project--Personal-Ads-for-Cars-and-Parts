namespace Store.Services.Models.ProductViewModels
{
    using Store.Data.Models.Enums;
    using static Store.Data.ModelConstants;
    using System.ComponentModel.DataAnnotations;

    public class EditProductViewModel
    {
        public int Id { get; set; }

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
