namespace Store.Web.Models.InvoiceViewModels
{
    using AutoMapper;
    using Store.Data.Models;
    using Store.Helpers.Interfaces.Mapping;
    using System;
    using System.Collections.Generic;

    public class ListOrdersViewModel : IMapFrom<Invoice>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public DateTime IssueDate { get; set; }

        public ICollection<InvoiceProductViewModel> Products { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Invoice, ListOrdersViewModel>()
                .ForMember(i => i.Products, options => options.MapFrom(i => i.InvoiceProducts));
        }
    }
}
