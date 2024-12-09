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
    public partial class UpdateChequeViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject, INavigationAware
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
        /// شناسه سند
        /// </summary>
        [ObservableProperty]
        private Guid _docId;

        /// <summary>
        /// وضعیت ثبت
        /// </summary>
        [ObservableProperty]
        private Dictionary<Enum, string> _enumSource;

        /// <summary>
        /// نام مشتری
        /// </summary>
        [ObservableProperty]
        private string _cusName;

        /// <summary>
        /// شماره مشتری
        /// </summary>
        [ObservableProperty]
        private string _cusNum;

        [ObservableProperty]
        private DateTime? _submitDate = DateTime.Now;

        /// <summary>
        /// تاریخ سررسید
        /// </summary>
        [ObservableProperty]
        private DateTime? _dueDate = DateTime.Now;

        /// <summary>
        /// نام صفحه
        /// </summary>
        [ObservableProperty]
        private string _pageName = "ویرایش";

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
        /// نوع سند 
        /// </summary>
        [ObservableProperty]
        private ChequeStatus _status;

        public async void OnNavigatedTo()
        {
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

            if (Status == ChequeStatus.Guarantee && Substatus != SubmitChequeStatus.NoNeedRegister && DueDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ سررسید"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (Status != ChequeStatus.Guarantee && DueDate == null)
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

            #region UpdatePayDocumetn
            using UnitOfWork db = new();
            var (e, s) = await db.DocumentManager.UpdateCheque(DocId, CusId.Value, Substatus, Description, SubmitDate.Value, DueDate, Price.Value, Cheque_Number, Accunt_Number, Bank_Name, Bank_Branch, Cheque_Owner);
            if (!s)
            {
                _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            #endregion

            #region UpdateNotif
            if (Status != ChequeStatus.Guarantee)
            {
                PersianCalendar pc = new();
                using BaseUnitOfWork baseDb = new();
                var (er, i) = await baseDb.NotifRepository.UpdateNotif(DocId, DueDate.Value.ToShamsiDate(pc) + " به مبلغ " + Price.Value.ToString("N0"), DueDate.Value);
                if (!i)
                {
                    _snackbarService.Show("خطا", er, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                }
            }
            #endregion


            #region ReDirect
            _snackbarService.Show("کاربر گرامی", $"ویرایش چک با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

            Type? pageType = NameToPageTypeConverter.Convert("Chequebook");

            if (pageType == null)
            {
                return;
            }
            _navigationService.Navigate(pageType);
            #endregion
        }
    }
}