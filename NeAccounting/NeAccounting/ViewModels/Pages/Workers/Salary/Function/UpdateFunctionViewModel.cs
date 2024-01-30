using DomainShared.Errore;
using DomainShared.ViewModels.Workers;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;
using System.Windows.Media;

namespace NeAccounting.ViewModels
{
    public partial class UpdateFunctionViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;

        public UpdateFunctionViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
        }

        [ObservableProperty]
        private int? _personnelId;

        [ObservableProperty]
        private string _personnelName;

        [ObservableProperty]
        private int _workerId = -1;

        [ObservableProperty]
        private int _salaryId = -1;

        [ObservableProperty]
        private int _funcId = -1;

        [ObservableProperty]
        private byte _amountOf = 0;

        [ObservableProperty]
        private byte _overTime = 0;

        [ObservableProperty]
        private DateTime _payDate = DateTime.Now;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private IEnumerable<FunctionViewModel> _list;

        public void OnNavigatedTo()
        {

        }


        public void OnNavigatedFrom()
        {

        }


        [RelayCommand]
        private async Task OnUpdate()
        {

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
                var (error, isSuccess) = await db.functionManager.UpdateFunc(WorkerId, SalaryId, FuncId, AmountOf, OverTime, Description);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(4000));
                    return;
                }
                await db.SaveChangesAsync();
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
