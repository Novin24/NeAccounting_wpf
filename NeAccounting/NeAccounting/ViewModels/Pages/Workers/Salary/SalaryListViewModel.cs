using DomainShared.Constants;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class SalaryListViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;

        #region Properties

        /// <summary>
        /// شناسه کارگر
        /// </summary>
        [ObservableProperty]
        private Guid? _workerId = null;

        [ObservableProperty]
        private int _currentPage = 1;

        [ObservableProperty]
        private int _pageCount = 1;

        [ObservableProperty]
        private string _totalCount;

        [ObservableProperty]
        private byte? _startMonth;

        [ObservableProperty]
        private int? _startYear;

        [ObservableProperty]
        private byte? _endMonth;

        [ObservableProperty]
        private int? _endYear;

        /// <summary>
        /// سرچ پرسنل
        /// </summary>
        [ObservableProperty]
        private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;

        /// <summary>
        /// لیست فیش حقوقی ها
        /// </summary>
        [ObservableProperty]
        private IEnumerable<SalaryViewModel> _list;

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

        public SalaryListViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService contentDialogService)
        {
            _snackbarService = snackbarService;
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
        }

        public void OnNavigatedFrom()
        {
        }

        public async void OnNavigatedTo()
        {
            await InitializeViewModel();
        }

        private async Task InitializeViewModel()
        {
            using UnitOfWork db = new();
            AuSuBox = await db.WorkerManager.GetDisplayWorkers();
            var salaries = await db.WorkerManager.GetSalaryList(WorkerId, StartMonth, StartYear, EndMonth, EndYear, CurrentPage);
            CurrentPage = salaries.CurrentPage;
            PageCount = salaries.PageCount;
            TotalCount = salaries.TotalCount.ToString("N0");
            List = salaries.Items;
            Loding = false;
            PlaceholderSearch = "جستجو ...";
        }

        [RelayCommand]
        private async Task OnSearchWorker()
        {
            using UnitOfWork db = new();
            var salaries = await db.WorkerManager.GetSalaryList(WorkerId, StartMonth, StartYear, EndMonth, EndYear, CurrentPage);
            CurrentPage = salaries.CurrentPage;
            PageCount = salaries.PageCount;
            TotalCount = salaries.TotalCount.ToString("N0");
            List = salaries.Items;
        }

        [RelayCommand]
        private async Task OnPageChenge()
        {
            using UnitOfWork db = new();
            var salaries = await db.WorkerManager.GetSalaryList(WorkerId, StartMonth, StartYear, EndMonth, EndYear, CurrentPage);
            PageCount = salaries.PageCount;
            TotalCount = salaries.TotalCount.ToString("N0");
            List = salaries.Items;
        }

        /// <summary>
        /// ثبت فیش حقوقی
        /// </summary>
        /// <param name="parameter"></param>
        [RelayCommand]
        private void OnAddClick(string parameter)
        {
            if (String.IsNullOrWhiteSpace(parameter))
            {
                return;
            }

            Type? pageType = NameToPageTypeConverter.Convert(parameter);

            if (pageType == null)
            {
                return;
            }

            _ = _navigationService.Navigate(pageType);
        }

        /// <summary>
        /// حذف فیش حقوقی
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnRemove(SalaryDetails parameter)
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            var result = await _contentDialogService.ShowSimpleDialogAsync(
            new SimpleContentDialogCreateOptions()
            {
                Title = "آیا از حذف اطمینان دارید!!!",
                Content = Application.Current.Resources["DeleteDialogContent"],
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });

            if (result == ContentDialogResult.Primary)
            {
                using UnitOfWork db = new();
                var isSuccess = await db.WorkerManager.DeleteSalary(parameter.WorkerId, parameter.Id);
                if (!isSuccess.isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", isSuccess.error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }

                _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                await OnSearchWorker();
            }
        }

        /// <summary>
        /// بروزرسانی فیش حقوقی
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnUpdate(SalaryDetails parameter)
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            Type? pageType = NameToPageTypeConverter.Convert("UpdateSalary");

            if (pageType == null)
            {
                return;
            }
            var service = _navigationService.GetNavigationControl();

            using UnitOfWork db = new();
            var (i, s) = await db.WorkerManager.GetSalaryDetailBySalaryId(parameter.WorkerId, parameter.Id, parameter.PersianMonth, parameter.PersianYear);
            if (!i)
            {
                _snackbarService.Show("کاربر گرامی", "فیش مورد نظر یافت نشد !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            var context = new UpdateSalaryPage(new UpdateSalaryViewModel(_snackbarService, _navigationService)
            {
                WorkerId = parameter.WorkerId,
                SalaryId = parameter.Id,
                AmountOf = s.AmountOf,
                OverTime = s.OverTime,
                Description = s.Description,
                PersonnelName = s.WorkerName,
                SubmitMonth = s.SubmitMonth,
                SubmitYear = s.SubmitYear,
                RightHousingAndFood = s.RightHousingAndFood,
                ShiftStatus = s.ShiftStatus,
                ChildAllowance = s.ChildAllowance,
                FinancialAid = s.FinancialAid,
                PersonnelId = s.PersonelId,
                Insurance = s.Insurance,
                LoanInstallment = s.LoanInstallment,
                Tax = s.Tax,
                OtherAdditions = s.OtherAdditions,
                OtherDeductions = s.OtherDeductions,
                LeftOver = s.LeftOver,
            });

            service.Navigate(pageType, context);
        }

        /// <summary>
        /// چاپ فیش حقوقی
        /// </summary>
        /// <param name="salaryId"></param>
        /// <returns></returns>
        public async Task<(bool isSuccess, SalaryWorkerViewModel item)> PrintSalary(int salaryId)
        {
            using UnitOfWork db = new();
            var details = List.First(t => t.Details.Id == salaryId).Details;
            var (i, s) = await db.WorkerManager.GetSalaryDetailBySalaryId(salaryId, details.PersianMonth, details.PersianYear);
            if (!i)
            {
                _snackbarService.Show("کاربر گرامی", "فیش مورد نظر یافت نشد !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return new(i, new SalaryWorkerViewModel());
            }
            return (i, s);
        }
    }
}
