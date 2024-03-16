using System.ComponentModel.DataAnnotations;

namespace DomainShared.Enums.StatusCheque
{
    public enum ChequeStatus : byte
    {
        [Display(Name = "ثبت شده")]
        Register = 1,
        [Display(Name = "ثبت شده")]
        NotRegister = 2,
        [Display(Name = "ثبت شده")]
        NoNeedRegister = 3,
    }
}