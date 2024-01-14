using DomainShared.Errore;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class UpdateFinancialAidViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;

        public UpdateFinancialAidViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
        }

        [ObservableProperty]
        private int? _PersonnelId;

        [ObservableProperty]
        private string _PersonnelName;

        [ObservableProperty]
        private int _workerId = -1;

        [ObservableProperty]
        private int _salaryId = -1;

        [ObservableProperty]
        private int _aidId = -1;

        [ObservableProperty]
        private uint _amountOf = 0;

        [ObservableProperty]
        private DateTime _payDate = DateTime.Now;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private IEnumerable<AidViewModel> _list;

        public void OnNavigatedTo()
        {
        }


        public void OnNavigatedFrom()
        {

        }

        [RelayCommand]
        private async Task OnUpdate()
        {

            if (AmountOf <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ مساعده"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.workerManager.UpdateAid(WorkerId, SalaryId, AidId, AmountOf, Description);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                    return;
                }
                await db.SaveChangesAsync();
            }

            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(2000));

            Type? pageType = NameToPageTypeConverter.Convert("FinancialAidList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }

    }
}
