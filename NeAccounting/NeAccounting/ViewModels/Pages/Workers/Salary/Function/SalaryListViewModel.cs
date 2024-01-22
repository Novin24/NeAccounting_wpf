using DomainShared.ViewModels;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class SalaryListViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;


        [ObservableProperty]
        private int _workerId = -1;

        [ObservableProperty]
        private DateTime? _startDate;

        [ObservableProperty]
        private DateTime? _endDate;

        [ObservableProperty]
        private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;

        [ObservableProperty]
        private IEnumerable<SalaryViewModel> _list;

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
            AuSuBox = await db.workerManager.GetWorkers();
            List = await db.workerManager.GetSalaryList(WorkerId, StartDate, EndDate);
        }

        [RelayCommand]
        private async Task OnSearchWorker()
        {
            using UnitOfWork db = new();
            List = await db.workerManager.GetSalaryList(WorkerId, StartDate, EndDate);
        }

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

        [RelayCommand]
        private async Task OnRemove(SalaryDetails parameter)
        {
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
                //using UnitOfWork db = new();
                //var isSuccess = await db.workerManager.DeleteFunc(parameter.WorkerId, parameter.SalaryId, parameter.Id);
                //if (!isSuccess.isSuccess)
                //{
                //    _snackbarService.Show("کاربر گرامی", isSuccess.error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(3000));
                //    return;
                //}
                //_snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                //await OnSearchWorker();
            }
        }

        [RelayCommand]
        private void OnUpdate(SalaryDetails parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("UpdateFunction");

            if (pageType == null)
            {
                return;
            }
            var service = _navigationService.GetNavigationControl();

            //var func = List.First(t => t.Details.Id == parameter.Id);

            //var context = new UpdateFunctionPage(new UpdateFunctionViewModel(_navigationService, _snackbarService)
            //{
            //    WorkerId = parameter.WorkerId,
            //    AmountOf = func.Amountof,
            //    Description = func.Description,
            //    PersonnelName = func.Name,
            //    SalaryId = parameter.SalaryId,
            //    OverTime = func.OverTime,
            //    FuncId = parameter.Id,
            //    PayDate = func.Date,
            //    List = List.Where(t => t.Details.WorkerId == parameter.WorkerId).OrderByDescending(c => c.Date).Take(10).ToList(),
            //    PersonnelId = func.PersonelId
            //});

            //service.Navigate(pageType, context);
        }
    }
}
