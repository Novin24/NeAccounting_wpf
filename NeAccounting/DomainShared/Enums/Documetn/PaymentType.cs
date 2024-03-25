using System.ComponentModel.DataAnnotations;

namespace DomainShared.Enums
{
    public enum PaymentType : byte
    {
        [Display(Name = "کارت به کارت")]
        CardToCard = 1,
        [Display(Name = "حواله بانکی")]
        BankInvoice = 2,
        [Display(Name = "شبا,ساتنا")]
        Sheba_Satna = 3,
        [Display(Name = "کارت خوان")]
        Pos = 4,
        [Display(Name = "نقدی")]
        Cash,
        [Display(Name = "چک")]
        Cheque,
        [Display(Name = "چک ضمانتی")]
        GurantyCheque,
        [Display(Name = "سایر")]
        Other
    }
}