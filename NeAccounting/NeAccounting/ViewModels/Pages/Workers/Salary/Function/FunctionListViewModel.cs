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
    public partial class FunctionListViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;


        [ObservableProperty]
        private int _workerId = -1;

        [ObservableProperty]
        private int _pageNum ;

        [ObservableProperty]
        private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;

        [ObservableProperty]
        private IEnumerable<FunctionViewModel> _list;

        public FunctionListViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService contentDialogService)
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
            AuSuBox = await db.WorkerManager.GetWorkers();
            List = await db.WorkerManager.GetFunctionList(WorkerId, PageNum);
        }

        [RelayCommand]
        private async Task OnSearchWorker()
        {
            using UnitOfWork db = new();
            List = await db.WorkerManager.GetFunctionList(WorkerId, PageNum);
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
        private async Task OnRemoveAid(FucntionDetails parameter)
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
                using UnitOfWork db = new();
                var isSuccess = await db.WorkerManager.DeleteFunc(parameter.WorkerId, parameter.persianYear, parameter.persianMonth, parameter.Id);
                if (!isSuccess.isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", isSuccess.error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                await db.SaveChangesAsync();
                _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                await OnSearchWorker();
            }
        }

        [RelayCommand]
        private async Task OnUpdateFunc(FucntionDetails parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("UpdateFunction");

            if (pageType == null)
            {
                return;
            }
            var service = _navigationService.GetNavigationControl();

            var func = List.First(t => t.Details.Id == parameter.Id);
            using UnitOfWork db = new();
            var list = await db.WorkerManager.GetFunctionList(WorkerId, PageNum);

            var context = new UpdateFunctionPage(new UpdateFunctionViewModel(_navigationService, _snackbarService)
            {
                WorkerId = parameter.WorkerId,
                AmountOf = func.Amountof,
                Description = func.Description,
                PersonnelName = func.Name,
                OverTime = func.OverTime,
                FuncId = parameter.Id,
                SubmitMonth = func.PersianMonth,
                SubmitYear = func.PersianYear,
                List = list,
                PersonnelId = func.PersonelId
            });

            service.Navigate(pageType, context);
        }
    }
}
