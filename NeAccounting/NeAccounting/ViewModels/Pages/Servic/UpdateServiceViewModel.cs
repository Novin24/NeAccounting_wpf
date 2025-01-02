using DomainShared.Constants;
using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore.Metadata;
using NeAccounting.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class UpdateServiceViewModel : ObservableObject, INavigationAware
    {

        private bool _isInitialized = false;
        private bool _isreadonly = true;
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        public UpdateServiceViewModel(ISnackbarService snackbarService, INavigationService navigationService)
        {
            _snackbarService = snackbarService;
            _navigationService = navigationService;
            _isreadonly = NeAccountingConstants.ReadOnlyMode;

        }
        [ObservableProperty]
        private IEnumerable<SuggestBoxViewModel<Guid>> _asuBox;

        /// <summary>
        /// عنوان کار
        /// </summary>
        [ObservableProperty]
        private string? _srvicName;

        /// <summary>
        /// اجرت کار
        /// </summary>
        [ObservableProperty]
        private long? _price;

        [ObservableProperty]
        private Guid? _unitId;

        [ObservableProperty]
        private Guid _materialId;

        [ObservableProperty]
        private string _address = string.Empty;

        public void OnNavigatedFrom()
        {

        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }
        [RelayCommand]
        private async Task OnUpdate()
        {

            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(SrvicName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("عنوان خدمت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (UnitId == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("واحد خدمت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (Price == null || Price == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("اجرت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            using UnitOfWork db = new();
            (string error, bool isSuccess) = await db.MaterialManager.UpdateMaterial(MaterialId, SrvicName, UnitId.Value, string.Empty, Address, Price.Value, false,0);

            if (!isSuccess)
            {
                _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            _snackbarService.Show("کاربرگرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20),
                TimeSpan.FromMilliseconds(3000));

            Type? pageType = NameToPageTypeConverter.Convert("MaterailList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);

        }
    }
}
