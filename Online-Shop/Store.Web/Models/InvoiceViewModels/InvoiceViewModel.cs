namespace Store.Web.Models.InvoiceViewModels
{
    using AutoMapper;
    using Store.Data.Models;
    using Store.Infrastructure.Mapping.Interfaces;
    using System.Collections.Generic;

    public class InvoiceViewModel : IMapFrom<Invoice>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string BuyerId { get; set; }

        public string BuyerFirstName { get; set; }

        public string BuyerLastName { get; set; }

        public string BuyeerStreet { get; set; }

        public string BuyeerTown { get; set; }

        public string BuyeerPostCode { get; set; }

        public string BuyeerCountry { get; set; }

        public decimal TotalValue { get; set; }

        public decimal NetValue { get; set; }

        public bool IsPayed { get; set; }

        public ICollection<InvoiceProductViewModel> Products { get; set; } = new HashSet<InvoiceProductViewModel>();

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Invoice, InvoiceViewModel>()
                .ForMember(i => i.BuyerFirstName, options => options.MapFrom(i => i.Buyer.FirstName))
                .ForMember(i => i.BuyerLastName, options => options.MapFrom(i => i.Buyer.LastName))
                .ForMember(i => i.BuyeerStreet, options => options.MapFrom(i => i.Buyer.Address.Street))
                .ForMember(i => i.BuyeerTown, options => options.MapFrom(i => i.Buyer.Address.Town.Name))
                .ForMember(i => i.BuyeerPostCode, options => options.MapFrom(i => i.Buyer.Address.Town.PostCode))
                .ForMember(i => i.BuyeerCountry, options => options.MapFrom(i => i.Buyer.Address.Town.Country.Name))
                .ForMember(i => i.Products, options => options.MapFrom(i => i.InvoiceProducts));
        }
    }
}
