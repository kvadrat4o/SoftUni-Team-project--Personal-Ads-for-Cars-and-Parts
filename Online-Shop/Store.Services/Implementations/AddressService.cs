namespace Store.Services.Implementations
{
    using System;
    using Store.Data;
    using Store.Data.Models;
    using Store.Services.Interfaces;
    using System.Linq;
    using Store.Services.Models.AddressViewModels;

    public class AddressService : IAddressService
    {
        private StoreDbContext db;

        public AddressService(StoreDbContext db)
        {
            this.db = db;
        }

        public Address GetAddress(SetAddressViewModel model)
        {
            var country = this.GetCountry(model.CountryName);
            var town = this.GetTown(model.TownName);

            var areAvailable = town.Id > 0 && country.Id > 0;
            if (!areAvailable)
            {
                town.Country = country;
                town.PostCode = model.PostCode;
            }

            Address address = null;
            if (areAvailable)
            {
                address = this.db.Addresses
                .FirstOrDefault(a => a.Street.Equals(model.Street, StringComparison.OrdinalIgnoreCase) &&
                    a.TownId == town.Id);
            }

            if (address == null)
            {
                address = new Address
                {
                    Street = model.Street,
                    Town = town
                };
            }

            return address;
        }

        private Town GetTown(string townName)
        {
            var town = this.db.Towns
                .FirstOrDefault(t => t.Name.Equals(townName, StringComparison.OrdinalIgnoreCase));

            if (town == null)
            {
                town = new Town { Name = townName };
            }

            return town;
        }

        private Country GetCountry(string countryName)
        {
            var country = this.db.Countries
                .FirstOrDefault(c => c.Name.Equals(countryName, StringComparison.OrdinalIgnoreCase));

            if (country == null)
            {
                country = new Country
                {
                    Name = countryName
                };
            }

            return country;
        }
    }
}
