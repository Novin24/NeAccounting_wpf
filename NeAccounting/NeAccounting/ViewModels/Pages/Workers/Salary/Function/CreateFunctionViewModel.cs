using DomainShared.Errore;
using DomainShared.ViewModels.Workers;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class CreateFunctionViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;

        public CreateFunctionViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
        }

        [ObservableProperty]
        private int? _PersonelId;

        [ObservableProperty]
        private int _workerId = -1;

        [ObservableProperty]
        private byte _amountOf = 0;

        [ObservableProperty]
        private byte _overTime = 0;

        [ObservableProperty]
        private DateTime _payDate = DateTime.Now;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;

        [ObservableProperty]
        private IEnumerable<FunctionViewModel> _list;

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
            AuSuBox = await db.workerManager.GetWorkers();
            List = await db.workerManager.GetFunctionList(WorkerId);
        }

        [RelayCommand]
        private async Task OnCreate()
        {

            if (WorkerId < 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام پرسنل"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (AmountOf < 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("کارکرد"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (OverTime < 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("اضافه کاری"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.workerManager.AddOrUpdateFunctuion(WorkerId, PayDate, AmountOf, OverTime, Description);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                    return;
                }
                await db.SaveChangesAsync();
            }

            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(2000));

            Type? pageType = NameToPageTypeConverter.Convert("FunctionList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }

    }
}
