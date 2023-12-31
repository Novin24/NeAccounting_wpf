using DomainShared.ViewModels;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class AidListViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;


        [ObservableProperty]
        private int _workerId = -1;


        [ObservableProperty]
        private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;

        [ObservableProperty]
        private IEnumerable<AidViewModel> _list;

        public AidListViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService contentDialogService)
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
            List = await db.workerManager.GetAidList();
        }

        [RelayCommand]
        private async Task OnSearchWorker()
        {
            using UnitOfWork db = new();
            List = await db.workerManager.GetAidList(WorkerId);
        }

        [RelayCommand]
        private void OAddClick(string parameter)
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
        private async Task OnRemoveAid(AidDetails parameter)
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
                var isSuccess = await db.workerManager.DeleteAsync(parameter);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", "خطا دراتصال به پایگاه داده!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                    return;
                }
                _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(2000));

                await OnSearchWorker();
            }
        }

        [RelayCommand]
        private async Task OnUpdateAid(AidDetails parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("UpdateFinancialAid");

            if (pageType == null)
            {
                return;
            }
            var service = _navigationService.GetNavigationControl();

            var aid = List.First(t => t.Details.Id == parameter.Id);

            var context = new UpdateFinancialAidPage(new UpdateFinancialAidViewModel(_navigationService, _snackbarService)
            {
                WorkerId = parameter.WorkerId,
                AmountOf = aid.AmountPrice,
                Description = aid.Description,
                PersonnelName = aid.Name,
                SalaryId = parameter.SalaryId,
                AidId = parameter.Id,
                PayDate = aid.Date,
                List = List.Where(t => t.Details.WorkerId == parameter.WorkerId),
                PersonnelId = aid.PersonelId
            });

            service.Navigate(pageType, context);
        }
    }
}
