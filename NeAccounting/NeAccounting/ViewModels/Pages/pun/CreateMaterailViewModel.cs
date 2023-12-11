using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels.Pages
{
    public partial class CreateMaterailViewModel : ObservableObject, INavigationAware
    {

        private bool _isInitialized = false;
        private readonly ISnackbarService _snackbarService;
        public CreateMaterailViewModel(ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;
        }

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
        private async Task OnCreateMaterial()
        {
            if (string.IsNullOrEmpty(MaterialName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کالا"), ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Serial))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("سریال کالا"), ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Address))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مکان فیزیکی کالا"), ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            using UnitOfWork db = new();
            var result = await db.materialManager.CreateMaterial(MaterialName, Entity, UnitId, Serial, Address);

            if (!result.isSuccess)
            {
                _snackbarService.Show("خطا", result.error, ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }


        }
    }
}
