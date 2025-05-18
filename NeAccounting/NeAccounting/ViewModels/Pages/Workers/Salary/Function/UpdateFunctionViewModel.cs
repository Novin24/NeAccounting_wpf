using DomainShared.Errore;
using DomainShared.ViewModels.Workers;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;
using System.Windows.Media;
using DomainShared.Constants;

namespace NeAccounting.ViewModels
{
    public partial class UpdateFunctionViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private bool _isreadonly = true;

        public UpdateFunctionViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
            _isreadonly = NeAccountingConstants.ReadOnlyMode;

        }

        [ObservableProperty]
        private int? _personnelId;

        [ObservableProperty]
        private string _personnelName;

        [ObservableProperty]
        private Guid _workerId;

        [ObservableProperty]
        private int _funcId = -1;

        [ObservableProperty]
        private byte _amountOf = 0;

        [ObservableProperty]
        private byte _overTime = 0;

        [ObservableProperty]
        private byte? _submitMonth;

        [ObservableProperty]
        private int? _submitYear;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private IEnumerable<FunctionViewModel> _list;

        [ObservableProperty]
        private bool _hasSalary;

        [ObservableProperty]
        private int _functionLimit;

        public void OnNavigatedTo()
        {
            if (!_hasSalary)
            {
                _snackbarService.Show("خطا", "برای ماه مورد نظر فیش حقوقی صادر شده!!!\n در صورت نیاز به ویرایش ابتدا فیش حقوقی ماه مرتبط را حذف کرده و مجددا تلاش نمایید.", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));

            }
        }


        public void OnNavigatedFrom()
        {

        }


        [RelayCommand]
        private async Task OnUpdate()
        {

            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (AmountOf < 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("کارکرد"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (AmountOf > FunctionLimit)
            {
                _snackbarService.Show("خطا", "کارکرد بیشتر از سقف مجاز", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
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

            if (SubmitMonth == null || SubmitYear == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پرداخت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.WorkerManager.UpdateFunc(WorkerId, SubmitYear.Value, SubmitMonth.Value, FuncId, AmountOf, OverTime, Description);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(4000));
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
    }
}
