namespace Store.Services.Models.ProductViewModels
{
    using System;
    using AutoMapper;
    using Store.Data.Models;
    using Store.Helpers.Interfaces.Mapping;

    public class ListOrderedProductViewModel : IMapFrom<ProductInvoice>, IHaveCustomMapping
    {
        public string SellerId { get; set; }

        public int InvoiceId { get; set; }

        public int ProductId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string PicturePath { get; set; }

        public bool IsNew { get; set; }

        public int Quantity { get; set; }

        public DateTime OrderDate { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<ProductInvoice, ListOrderedProductViewModel>()
                .ForMember(m => m.SellerId, options => options.MapFrom(pi => pi.Product.SellerId))
                .ForMember(m => m.Title, options => options.MapFrom(pi => pi.Product.Title))
                .ForMember(m => m.Price, options => options.MapFrom(pi => pi.Product.Price))
                .ForMember(m => m.PicturePath, options => options.MapFrom(pi => pi.Product.PicturePath ?? ServiceConstants.DefaultProductImage))
                .ForMember(m => m.IsNew, options => options.MapFrom(pi => pi.Product.IsNew))
                .ForMember(m => m.OrderDate, options => options.MapFrom(pi => pi.Invoice.IssueDate));
        }
    }
}
