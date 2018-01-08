namespace Store.Services.Models.ProductViewModels
{
    using System;
    using AutoMapper;
    using Store.Data.Models;
    using Store.Helpers.Interfaces.Mapping;

    public class SoldProductViewModel : IMapFrom<ShippingRecord>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public DateTime? DispatchDate { get; set; }

        public int ProductId { get; set; }

        public int InvoiceId { get; set; }

        public int Quantity { get; set; }

        public DateTime OrderDate { get; set; }

        public string SellerId { get; set; }

        public string BuyerId { get; set; }

        public string Title { get; set; }

        public bool IsNew { get; set; }

        public string PicturePath { get; set; }

        public decimal ProductPrice { get; set; }

        public string BuyerUserName { get; set; }

        public string BuyerName { get; set; }

        public string BuyerStreet { get; set; }

        public string BuyerTown { get; set; }

        public string BuyerPostCode { get; set; }

        public string BuyerCountry { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<ShippingRecord, SoldProductViewModel>()
                .ForMember(m => m.OrderDate, options => options.MapFrom(sr => sr.Invoice.IssueDate))
                .ForMember(m => m.SellerId, options => options.MapFrom(sr => sr.Product.SellerId))
                .ForMember(m => m.BuyerId, options => options.MapFrom(sr => sr.Invoice.BuyerId))
                .ForMember(m => m.Title, options => options.MapFrom(sr => sr.Product.Title))
                .ForMember(m => m.IsNew, options => options.MapFrom(sr => sr.Product.IsNew))
                .ForMember(m => m.PicturePath, options => options.MapFrom(sr => sr.Product.PicturePath ?? ServiceConstants.DefaultProductImage))
                .ForMember(m => m.ProductPrice, options => options.MapFrom(sr => sr.Product.Price))
                .ForMember(m => m.BuyerUserName, options => options.MapFrom(sr => sr.Invoice.Buyer.UserName))
                .ForMember(m => m.BuyerName, options => options.MapFrom(sr => $"{sr.Invoice.Buyer.FirstName} {sr.Invoice.Buyer.LastName}"))
                .ForMember(m => m.BuyerStreet, options => options.MapFrom(sr => sr.Invoice.Buyer.Address.Street))
                .ForMember(m => m.BuyerStreet, options => options.MapFrom(sr => sr.Invoice.Buyer.Address.Town.Name))
                .ForMember(m => m.BuyerPostCode, options => options.MapFrom(sr => sr.Invoice.Buyer.Address.Town.PostCode))
                .ForMember(m => m.BuyerCountry, options => options.MapFrom(sr => sr.Invoice.Buyer.Address.Town.Country.Name));
        }
    }
}
