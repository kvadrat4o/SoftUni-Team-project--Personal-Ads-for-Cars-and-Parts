namespace Store.Infrastructure.Mapping.Interfaces
{
    using AutoMapper;

    public interface IHaveCustomMapping
    {
        void ConfigureMapping(Profile mapper);
    }
}
