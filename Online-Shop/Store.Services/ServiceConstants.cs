namespace Store.Services
{
    internal class ServiceConstants
    {
        internal const decimal ProductListingPriceTax = 0.65M;

        internal const string UserDirectory = "../Store.Web/wwwroot/images/{0}";
        internal const string PathToRemove = "Store.Web/wwwroot/";

        internal const long ImageMaxSizeInMB = 1;
        internal const long ImageMaxSizeInBytes = ImageMaxSizeInMB * 1024 * 1024;
    }
}
