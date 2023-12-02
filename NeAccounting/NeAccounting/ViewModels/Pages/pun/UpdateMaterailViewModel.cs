using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels.Pages
{
    public partial class UpdateMaterailViewModel : ObservableObject, INavigationAware
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

        [ObservableProperty]
        private int _materialId;

        [ObservableProperty]
        private string _erroreMessage = "";

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
        private async Task OnUpdate()
        {
            if (string.IsNullOrEmpty(MaterialName))
            {
                ErroreMessage = NeErrorCodes.IsMandatory("نام کالا");
                return;
            }
            if (string.IsNullOrEmpty(Serial))
            {
                ErroreMessage = NeErrorCodes.IsMandatory("سریال کالا");
                return;
            }
            if (string.IsNullOrEmpty(Address))
            {
                ErroreMessage = NeErrorCodes.IsMandatory("مکان فیزیکی کالا");
                return;
            }

            using UnitOfWork db = new();
            await db.materialManager.UpdateMaterial(MaterialId, MaterialName, Entity, UnitId, Serial, Address);
        }
    }
}
