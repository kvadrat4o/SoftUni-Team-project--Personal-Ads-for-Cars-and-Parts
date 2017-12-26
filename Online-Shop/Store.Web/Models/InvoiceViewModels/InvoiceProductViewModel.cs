namespace Store.Web.Models.InvoiceViewModels
{
    using AutoMapper;
    using Store.Data;
    using Store.Data.Models;
    using Store.Infrastructure.Mapping.Interfaces;

    public class InvoiceProductViewModel : IMapFrom<ProductInvoice>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public decimal NetPrice => this.Price * (1 - ModelConstants.VAT / 100);

        public string PicturePath { get; set; } = WebConstants.FullLogoPath;

        public bool IsNew { get; set; }

        public int Quantity { get; set; }

        public string SellerId { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<ProductInvoice, InvoiceProductViewModel>()
                .ForMember(p => p.Id, options => options.MapFrom(p => p.Product.Id))
                .ForMember(p => p.Title, options => options.MapFrom(p => p.Product.Title))
                .ForMember(p => p.Price, options => options.MapFrom(p => p.Product.Price))
                .ForMember(p => p.PicturePath, options => options.MapFrom(p => p.Product.PicturePath))
                .ForMember(p => p.IsNew, options => options.MapFrom(p => p.Product.IsNew))
                .ForMember(p => p.SellerId, options => options.MapFrom(p => p.Product.SellerId));
        }
    }
}
