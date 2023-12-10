using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class MaterailListViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private string _punName = "";

        [ObservableProperty]
        private string _serial = "";

        [ObservableProperty]
        private IEnumerable<PunListDto> _list;
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
            List = await db.materialManager.GetMaterails(string.Empty, string.Empty);
        }

        [RelayCommand]
        private async Task OnSearchMaterial()
        {
            using UnitOfWork db = new();
            List = await db.materialManager.GetMaterails(PunName, Serial);
        }
    }
}
