using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.Extension;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Globalization;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;
namespace NeAccounting.ViewModels
{
    public partial class CreatePayChequeViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService = snackbarService;
        private readonly INavigationService _navigationService = navigationService;
        private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;


        #region Properties

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

        [ObservableProperty]
        private DateTime? _submitDate = DateTime.Now;

        /// <summary>
        /// تاریخ سررسید
        /// </summary>
        [ObservableProperty]
        private DateTime? _dueDate = DateTime.Now;

        /// <summary>
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
        /// سریال چک
        /// </summary>
        [ObservableProperty]
        private string? _cheque_Number;

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
        /// شماره شبا
        /// </summary>
        [ObservableProperty]
        private string _shaba_Number;

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
        private SubmitChequeStatus _status = SubmitChequeStatus.Register;

        /// <summary>
        /// غیرفعال بودن سرچ
        /// </summary>
        [ObservableProperty]
        private bool _loding = true;

        /// <summary>
        /// متن نمایشی سرچ
        /// </summary>
        [ObservableProperty]
        private string _placeholderSearch = "در حال بارگذاری ...";
        #endregion

        public async void OnNavigatedTo()
        {
            await InitializeViewModel();
        }

        private async Task InitializeViewModel()
        {
            using UnitOfWork db = new();
            Cuslist = await db.CustomerManager.GetDisplayUser();
            Loding = false;
            PlaceholderSearch = "جستجو ...";
        }

        public void OnNavigatedFrom()
        {
        }

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
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (DueDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ سررسید"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(Cheque_Owner))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("صاحب چک"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(Cheque_Number))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("شماره چک"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (Price == null || Price == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ چک"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(Bank_Name))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام بانک"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(Description))
            {
                Description = $"چک ({Cheque_Number}) پرداختی به مشتری";
            }

            #endregion

            #region CreatePayDocumetn&Cheque
            using UnitOfWork db = new();
            var (e, s, docId) = await db.DocumentManager.CreatePayCheque(CusId.Value, Status, Description, SubmitDate.Value, DueDate.Value, Price.Value, Cheque_Number, Cheque_Series, SiadyNumber, Shaba_Number, Bank_Name, Bank_Branch, Cheque_Owner);
            if (!s)
            {
                _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            _snackbarService.Show("کاربر گرامی", $"ثبت چک با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            #endregion

            #region CreateNotif
            PersianCalendar pc = new();
            using BaseUnitOfWork baseDb = new();
            var (er, i) = await baseDb.NotifRepository.CreateNotif(docId, "سررسید چک پرداختی ", DueDate.Value.ToShamsiDate(pc) + " به مبلغ " + Price.Value.ToString("N0"), Priority.Medium, DueDate.Value);
            if (!i)
            {
                _snackbarService.Show("خطا", er, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            }
            #endregion

            #region redirect
            Type? pageType = NameToPageTypeConverter.Convert("Chequebook");
            if (pageType == null)
            {
                return;
            }
            _navigationService.Navigate(pageType);
            return;
            #endregion

        }
    }
}