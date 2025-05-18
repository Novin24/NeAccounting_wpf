using DomainShared.Errore;
using DomainShared.ViewModels.Workers;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;
using System.Windows.Media;
using System.Globalization;
using DomainShared.Constants;
using DomainShared.Enums;

namespace NeAccounting.ViewModels
{
    public partial class CreateFunctionViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private bool _isreadonly = true;

        public CreateFunctionViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
            PersianCalendar pc = new();
            SubmitMonth = (byte)pc.GetMonth(DateTime.Now);
            SubmitYear = pc.GetYear(DateTime.Now);
            _isreadonly = NeAccountingConstants.ReadOnlyMode;
        }

        #region Properties
        [ObservableProperty]
        private int? _PersonelId;

        [ObservableProperty]
        private Guid? _workerId = null;

        [ObservableProperty]
        private byte _amountOf = 0;

        [ObservableProperty]
        private byte _overTime = 0;

        [ObservableProperty]
        private byte? _submitMonth;

        [ObservableProperty]
        private string _displayDate;

        [ObservableProperty]
        private int? _submitYear;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;

        [ObservableProperty]
        private IEnumerable<FunctionViewModel> _list;

        [ObservableProperty]
        private int _functionLimit;

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


        public void OnNavigatedFrom()
        {

        }

        private async Task InitializeViewModel()
        {
            using UnitOfWork db = new();
			AuSuBox = await db.WorkerManager.GetDisplayWorkers();
			var t = await db.WorkerManager.GetFunctionList(WorkerId);
			List = t.Items;
            Loding = false;
            PlaceholderSearch = "جستجو ...";
        }

        [RelayCommand]
        private async Task OnCreate()
        {

            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (WorkerId == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام پرسنل"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (AmountOf < 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("کارکرد"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (AmountOf > 31)
            {
                _snackbarService.Show("خطا", "کارکرد بیشتر از سقف مجاز", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (SubmitMonth == null || SubmitYear == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پرداخت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (OverTime < 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("اضافه کاری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (OverTime > 120)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsLess("اضافه کاری", "صدوبیست ساعت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.WorkerManager.AddFunctuion(WorkerId.Value, SubmitYear.Value, SubmitMonth.Value, AmountOf, OverTime, Description);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
            }

            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

            Type? pageType = NameToPageTypeConverter.Convert("FunctionList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }

        
        public async Task GetFunctionLimit(Guid? workerId)
        {
            using UnitOfWork db = new();
            var shiftStatus = await db.WorkerManager.GetWorkerShiftStatusById(workerId);
            FunctionLimit = shiftStatus == Shift.ByMounth ? 31 : 500;
        }
    }
}
