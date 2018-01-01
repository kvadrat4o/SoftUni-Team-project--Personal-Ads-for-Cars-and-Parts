namespace Store.Web
{
    public class WebConstants
    {
        public const string UserDefaultAvatarPath = "../images/DefaultProfilePicture.png";
        public const string FullLogoPath = "../images/FullLogo.png";
        public const string FaviconPath = "../images/favicon.ico";

        public const string OrderCompletedMessageText = "Order Completed. Thank you for your purchase. Your order will be shipped as soon as possible.";

        /* TempData Keys */
        public const string SuccessMessageKey = "Success";
        public const string InfoMessageKey = "Info";
        public const string WarningMessageKey = "Warning";
        public const string DangerMessageKey = "Danger";

        public const int InvoiceNumberLeadingZeroes = 10;
    }
}
