using DomainShared.Errore;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
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
        private int? _personnelId;

        [ObservableProperty]
        private string _personnelName;

        [ObservableProperty]
        private int _workerId = -1;

        [ObservableProperty]
        private byte? _submitMonth;

        [ObservableProperty]
        private int? _submitYear;

        [ObservableProperty]
        private int _aidId = -1;

        [ObservableProperty]
        private uint _amountOf = 0;

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
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ مساعده"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (SubmitMonth == null || SubmitYear == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پرداخت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.WorkerManager.UpdateAid(WorkerId, SubmitYear.Value, SubmitMonth.Value, AidId, AmountOf, Description);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                await db.SaveChangesAsync();
            }

            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

            Type? pageType = NameToPageTypeConverter.Convert("FinancialAidList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }

    }
}
