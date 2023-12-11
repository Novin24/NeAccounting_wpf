using DomainShared.Enums;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class WorkerListViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string _fullName = "";

        [ObservableProperty]
        private string _jobTitle = "";

        [ObservableProperty]
        private string _nationalCode = "";

        private Status Statu { get { return (Status)StatusByte; } set { StatusByte = (byte)value; } }

        [ObservableProperty]
        private byte _statusByte = 0;

        [ObservableProperty]
        private IEnumerable<WorkerVewiModel> _list;

        public WorkerListViewModel(ISnackbarService snackbarService, INavigationService navigationService)
        {
            _snackbarService = snackbarService;
            _navigationService = navigationService;
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
            List = await db.workerManager.GetWorkers(
                        FullName,
                        JobTitle,
                        NationalCode,
                        Statu);

            if (!List.Any())
                _snackbarService.Show("اوه", "هیچ کارگری در پایگاه داده یافت نشد!!!!", ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
        }

        [RelayCommand]
        private async Task OnSearchWorker()
        {
            using UnitOfWork db = new();
            List = await db.workerManager.GetWorkers(
                        FullName,
                        JobTitle,
                        NationalCode,
                        Statu);

            if (!List.Any())
                _snackbarService.Show("اوه", "هیچ کارگری در پایگاه داده با این مشخصات یافت نشد!!!!", ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
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
    }
}
