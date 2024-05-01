using DomainShared.Errore;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class MaterialReportViewModel : ObservableObject, INavigationAware
    {
        private bool _isInit;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;

        public MaterialReportViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
        }

        #region Properties

        [ObservableProperty]
        private long? _personelId;

        [ObservableProperty]
        private int _pageCount = 1;

        [ObservableProperty]
        private bool _sell = true;

        [ObservableProperty]
        private bool _buy = true;

        [ObservableProperty]
        private Guid? _materialId = null;

        [ObservableProperty]
        private int _currentPage = 1;

        [ObservableProperty]
        private DateTime? _startDate = DateTime.Now;

        [ObservableProperty]
        private DateTime? _endDate = DateTime.Now;

        /// <summary>
        /// لیست اجناس
        /// </summary>
        [ObservableProperty]
        private List<PunListDto> _matList;

        /// <summary>
        /// لیست اجناس
        /// </summary>
        [ObservableProperty]
        private IEnumerable<MaterialReportDto> _matReportList;
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
            using UnitOfWork db = new();
            MatList = await db.MaterialManager.GetMaterails("", "");
        }

        [RelayCommand]
        private async Task OnSearchInvoice()
        {

            if (MaterialId == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام جنس"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (StartDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ شروع"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (EndDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پایان"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (Sell == false && Buy == false)
            {
                _snackbarService.Show("خطا", "وضعیت خرید یا فروش را مشخص نمایید!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            _isInit = true;
            using UnitOfWork db = new();
            var t = await db.DocumentManager.GetMaterialReport(MaterialId.Value, Buy, Sell, StartDate.Value, EndDate.Value, false, _isInit);
            CurrentPage = t.CurrentPage;
            MatReportList = t.Items;
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
            if (MaterialId == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام جنس"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (StartDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ شروع"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (EndDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پایان"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (Sell == Buy == false)
            {
                _snackbarService.Show("خطا", "وضعیت خرید یا فروش را مشخص نمایید!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            using UnitOfWork db = new();
            var t = await db.DocumentManager.GetMaterialReport(MaterialId.Value, Buy, Sell, StartDate.Value, EndDate.Value, false, _isInit);
            CurrentPage = t.CurrentPage;
            MatReportList = t.Items;
            PageCount = t.PageCount;
        }
        #endregion
    }
}
