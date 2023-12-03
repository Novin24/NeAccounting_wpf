using DomainShared.Enums;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class WorkerListViewModel : ObservableObject, INavigationAware
    {
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
        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
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
        }
    }
}
