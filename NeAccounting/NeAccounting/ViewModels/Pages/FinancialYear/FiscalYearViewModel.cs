using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class FiscalYearViewModel : ObservableObject, INavigationAware
    {
        private bool _isInit;
        private readonly INavigationService _navigationService;
        private readonly ISnackbarService _snackbarService;

        public FiscalYearViewModel(INavigationService navigationService,ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
        }

        #region Properties

        [ObservableProperty]
        private int _pageCount = 1;

        [ObservableProperty]
        private int _currentPage = 1;


        /// <summary>
        /// لیست چک
        /// </summary>
        [ObservableProperty]
        private IEnumerable<FiscalYearDto> _yearList;
        #endregion

        #region Methods
        public async void OnNavigatedTo()
        {
            await InitializeViewModel();
        }
        public void OnNavigatedFrom()
        {

        }

        private async Task InitializeViewModel()
        {
            _isInit = true;
            using BaseUnitOfWork db = new();
            var t = await db.FinancialYearRepository.GetFiscalYears(_isInit, CurrentPage);
            CurrentPage = t.CurrentPage;
            YearList = t.Items;
            PageCount = t.PageCount;
            _isInit = false;
        }

        [RelayCommand]
        private async Task OnChangePage()
        {
            if (_isInit)
            {
                return;
            }

            using BaseUnitOfWork db = new();
            var t = await db.FinancialYearRepository.GetFiscalYears(_isInit, CurrentPage);
            YearList = t.Items;
            PageCount = t.PageCount;
        }

        [RelayCommand]
        private async Task OnChangeYear(Guid parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("Dashboard");

            if (pageType == null)
            {
                return;
            }

            using BaseUnitOfWork db = new();
            var (s, i) = await db.FinancialYearRepository.ChangeFinancialYear(parameter);
            if (!s)
            {
                _snackbarService.Show("خطا", i, ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
                return;
            }
            await db.SaveChangesAsync();
            _snackbarService.Show("کاربر گرامی", $"تعویض سال مالی با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            var servise = _navigationService.Navigate(pageType);
        }

        #endregion
    }
}




