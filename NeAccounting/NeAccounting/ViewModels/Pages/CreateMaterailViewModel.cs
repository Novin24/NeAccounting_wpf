using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels.Pages
{
    public partial class CreateMaterailViewModel : ObservableObject, INavigationAware
    {

        private bool _isInitialized = false;

        [ObservableProperty]
        private IEnumerable<SuggestBoxViewModel<Guid>> _asuBox;

        [ObservableProperty]
        private string _materialName;

        [ObservableProperty]
        private string _serial;

        [ObservableProperty]
        private string _address;

        [ObservableProperty]
        private double _entity;

        [ObservableProperty]
        private int _unitId;


        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                await InitializeViewModel();
        }

        private async Task InitializeViewModel()
        {
            using (UnitOfWork db = new())
            {
                await db.unitManager.GetUnits();
            }
            _isInitialized = true;
        }

        [RelayCommand]
        private async Task OnCreate()
        {
            if (string.IsNullOrEmpty(MaterialName))
                return;

            using UnitOfWork db = new();
            await db.materialManager.CreateMaterial(MaterialName, Entity, UnitId, Serial, Address);
        }


    }
}
