namespace Store.Web.Models.ManageViewModels
{
    using System.ComponentModel.DataAnnotations;
    using static Data.ModelConstants;

    public class AddMoneyToBalanceViewModel
    {
        [RegularExpression(@"([\d]{4}[\s]){3}[\d]{4}")]
        public string CreditCardNumber { get; set; }

        [RegularExpression(@"[\d]{3}")]
        public string CardSecurityCode { get; set; }

        [Range(MoneyMinValue, MoneyMaxValue)]
        public decimal MoneyBalance { get; set; }
    }
}
