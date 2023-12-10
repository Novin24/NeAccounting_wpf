using DomainShared.Enums;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class WorkerListViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;


        [ObservableProperty]
        private string _fullName = "";

        [ObservableProperty]
        private string _jobTitle = "";

        [ObservableProperty]
        private string _nationalCode = "";

        [ObservableProperty]
        private Status _status = Status.All;

        [ObservableProperty]
        private IEnumerable<WorkerVewiModel> _list;

        public WorkerListViewModel(ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;
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
                        Status);

            if (!List.Any())
                _snackbarService.Show("اوه","هیچ کارگری در پایگاه داده یافت نشد!!!!", ControlAppearance.Primary,new SymbolIcon(SymbolRegular.Warning20),TimeSpan.FromMilliseconds(2000));
        }
    }
}
