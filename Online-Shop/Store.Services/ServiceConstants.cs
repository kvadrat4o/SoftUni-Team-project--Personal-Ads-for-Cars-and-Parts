namespace Store.Services
{
    public class ServiceConstants
    {
        internal const decimal ProductListingPriceTax = 0.65M;

        internal const string UserDirectory = "../Store.Web/wwwroot/images/{0}";
        internal const string PathToRemove = "Store.Web/wwwroot/";
        public const string DefaultProductImage = "../images/FullLogo.png";

        internal const long ImageMaxSizeInMB = 1;
        internal const long ImageMaxSizeInBytes = ImageMaxSizeInMB * 1024 * 1024;

        public const int PageSize = 3;
    }
}
