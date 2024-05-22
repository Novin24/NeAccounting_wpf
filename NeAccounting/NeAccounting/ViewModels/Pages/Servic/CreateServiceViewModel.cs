using Domain.NovinEntity.Materials;
using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class CreateServiceViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;    
        private bool _isreadonly = true;

        private readonly INavigationService _navigationService;
        private readonly ISnackbarService _snackbarService;
        public CreateServiceViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
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
        private long _price;

        /// <summary>
        /// توضیحات
        /// </summary>
        [ObservableProperty]
        private string _address = string.Empty;

        [ObservableProperty]
        private Guid? _unitId;
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
                AsuBox = await db.UnitManager.GetUnits();
            }

            if (AsuBox.Any())
            {
                UnitId = AsuBox.First().Id;
            }

            _isInitialized = true;
        }
        [RelayCommand]
        private async Task OnCreateService()
        {
            if (string.IsNullOrEmpty(SrvicName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("عنوان خدمت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (UnitId == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("واحد کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.MaterialManager.CreateMaterial(SrvicName, UnitId.Value, true, Price, string.Empty, Address, false);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
            }

            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            Type? pageType = NameToPageTypeConverter.Convert("MaterailList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }

    }
}
