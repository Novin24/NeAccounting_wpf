using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;
namespace NeAccounting.ViewModels
{
    public partial class TransferChequeViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject
    {
        private readonly ISnackbarService _snackbarService = snackbarService;
        private readonly INavigationService _navigationService = navigationService;
        private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;

        /// <summary>
        /// لیست مشتری ها
        /// </summary>
        [ObservableProperty]
        private List<SuggestBoxViewModel<Guid, long>> _cuslist;

        /// <summary>
        /// شناسه مشتری
        /// </summary>
        [ObservableProperty]
        private Guid? _CusId;

        /// <summary>
        /// نام پرداخت کننده
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        /// شناسه سند
        /// </summary>
        [ObservableProperty]
        private Guid _docId;

        /// <summary>
        /// وضعیت ثبت
        /// </summary>
        [ObservableProperty]
        private Dictionary<Enum, string> _enumSource;


        [ObservableProperty]
        private DateTime? _submitDate = DateTime.Now;

        /// <summary>
        /// تاریخ سررسید
        /// </summary>
        [ObservableProperty]
        private DateTime? _dueDate = DateTime.Now;

        /// <summary>
        /// مبلغ چک 
        /// </summary>
        [ObservableProperty]
        private long? _price;

        /// <summary>
        /// توضیحات 
        /// </summary>
        [ObservableProperty]
        private string? _description;

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
		/// سری چک
		/// </summary>
		[ObservableProperty]
		private string? _cheque_Series;

		/// <summary>
		/// شماره صیادی
		/// </summary>
		[ObservableProperty]
		private string? _siadyNumber;

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
        /// نوع ثبت سند 
        /// </summary>
        [ObservableProperty]
        private SubmitChequeStatus _substatus = SubmitChequeStatus.NotRegister;

        /// <summary>
        /// ثبت فاکتور
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnSubmit()
        {
            #region validation
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (CusId == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (SubmitDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ واگذاری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(Description))
            {
                var rec = Cuslist.First(t => t.Id == CusId.Value);
                Description = $"چک ({Cheque_Number}) واگذاری از {PayerName} به {rec.DisplayName}";
            }

            #endregion

            #region CreatePayDocumetn
            using UnitOfWork db = new();
            var (e, s) = await db.DocumentManager.AssignCheque(DocId, CusId.Value, SubmitDate.Value, Description);
            if (s)
            {
                await db.SaveChangesAsync();
                _snackbarService.Show("کاربر گرامی", $"ویرایش چک با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                Type? pageType = NameToPageTypeConverter.Convert("Chequebook");

                if (pageType == null)
                {
                    return;
                }

                _navigationService.Navigate(pageType);
                return;
            }

            _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            #endregion
        }
    }

}