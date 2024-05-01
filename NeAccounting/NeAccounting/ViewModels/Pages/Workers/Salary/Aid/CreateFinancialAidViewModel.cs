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

namespace NeAccounting.ViewModels
{
    public partial class CreateFinancialAidViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private bool _isreadonly = true;

        public CreateFinancialAidViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
            _isreadonly = NeAccountingConstants.ReadOnlyMode;
            _isreadonly = NeAccountingConstants.ReadOnlyMode;

        }

        [ObservableProperty]
        private int? _PersonelId;

        [ObservableProperty]
        private Guid? _workerId = null;

        [ObservableProperty]
        private long _amountOf = 0;

        [ObservableProperty]
        private DateTime? _submitDate = DateTime.Now;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;

        [ObservableProperty]
        private IEnumerable<AidViewModel> _list;

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
            AuSuBox = await db.WorkerManager.GetWorkers();
            List = await db.WorkerManager.GetAidList(WorkerId);
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
            if (SubmitDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پرداخت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (AmountOf <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ مساعده"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            PersianCalendar pc = new();
            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.WorkerManager.AddAid(SubmitDate.Value, WorkerId.Value, pc.GetYear(SubmitDate.Value), (byte)pc.GetMonth(SubmitDate.Value), AmountOf, Description);
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
