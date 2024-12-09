using DomainShared.Enums;
using NeAccounting.Helpers;
using Wpf.Ui;
namespace NeAccounting.ViewModels
{
    public partial class DetailsChequeViewModel(INavigationService navigationService) : ObservableObject
    {
        private readonly INavigationService _navigationService = navigationService;

        /// <summary>
        /// وضعیت ثبت
        /// </summary>
        [ObservableProperty]
        private string _subStatus;

        /// <summary>
        /// نام مشتری دریافت کننده
        /// </summary>
        [ObservableProperty]
        private string _cusName;

        /// <summary>
        /// شماره مشتری دریافت کننده
        /// </summary>
        [ObservableProperty]
        private string _cusNum;

        /// <summary>
        /// نام مشتری پرداخت کننده
        /// </summary>
        [ObservableProperty]
        private string _payCusName;

        /// <summary>
        /// شماره مشتری پرداخت کننده
        /// </summary>
        [ObservableProperty]
        private string _payCusNum;

        /// <summary>
        /// تاریخ پرداخت یا دریافت
        /// </summary>
        [ObservableProperty]
        private string? _submitDate;

        /// <summary>
        /// تاریخ سررسید
        /// </summary>
        [ObservableProperty]
        private string? _dueDate;

        /// <summary>
        /// تاریخ واگذری
        /// </summary>
        [ObservableProperty]
        private string? _transferDate;

        /// <summary>
        /// مبلغ چک 
        /// </summary>
        [ObservableProperty]
        private string? _price;

        /// <summary>
        /// توضیحات 
        /// </summary>
        [ObservableProperty]
        private string? _recDescription;

        /// <summary>
        /// توضیحات 
        /// </summary>
        [ObservableProperty]
        private string? _payDescription;

        /// <summary>
        /// شماره چک
        /// </summary>
        [ObservableProperty]
        private string? _cheque_Number;


        /// <summary>
        /// شماره شبا
        /// </summary>
        [ObservableProperty]
        private string _accunt_Number;

        /// <summary>
        /// نام بانک
        /// </summary>
        [ObservableProperty]
        private string _bank_Name;

        /// <summary>
        /// نام شعبه
        /// </summary>
        [ObservableProperty]
        private string _bank_Branch;

        /// <summary>
        /// صاحب چک
        /// </summary>
        [ObservableProperty]
        private string cheque_Owner;

        /// <summary>
        /// نوع سند 
        /// </summary>
        [ObservableProperty]
        private ChequeStatus _status;

        [RelayCommand]
        private void OnBackClick(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return;
            }

            Type? pageType = NameToPageTypeConverter.Convert(parameter);

            if (pageType == null)
            {
                return;
            }

            _ = _navigationService.Navigate(pageType);
        }
    }
}