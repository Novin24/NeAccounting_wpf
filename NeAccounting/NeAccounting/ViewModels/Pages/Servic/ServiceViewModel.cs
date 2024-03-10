using Domain.NovinEntity.Materials;
using Domain.NovinEntity.Services;
using DomainShared.Errore;
using DomainShared.ViewModels.Service;
using Infrastructure.UnitOfWork;
using Microsoft.Identity.Client.NativeInterop;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class ServiceViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private readonly ISnackbarService _snackbarService;
        public ServiceViewModel(ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;
        }
        [ObservableProperty]
        private int? _servicId;
        [ObservableProperty]
        private string _servicName;
        [ObservableProperty]
        private int? _servicPrice;
        [ObservableProperty]
        private List<ServiceListDto> _list;
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
                List = await db.ServiceManager.GetServiceList();
            }

            _isInitialized = true;
        }
        [RelayCommand]
        private async Task OnCreateServic()
        {
            if (string.IsNullOrEmpty(ServicName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("عنوان کار"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (ServicPrice == null || ServicPrice == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("اجرت کار"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            using UnitOfWork db = new();
            if (ServicId != null)
            {

                var (error, isSuccess) = await db.ServiceManager.UpdateService(ServicId.Value, ServicName, ServicPrice.Value);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                await db.SaveChangesAsync();
                ServicId = null;
                ServicName = string.Empty;
                ServicPrice = null;

                _snackbarService.Show("کاربر گرامی", "عملیات ویرایش با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            }
            else
            {
                var (error, isSuccess) = await db.ServiceManager.CreateService(ServicName, ServicPrice.Value);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                await db.SaveChangesAsync();
                _snackbarService.Show("کاربر گرامی", "عملیات ثبت با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            }
            List = await db.ServiceManager.GetServiceList();
        }
        [RelayCommand]
        private async Task OnActive(int id)
        {
            using UnitOfWork db = new();
            await db.ServiceManager.ChangeStatus(id, true);
            await db.SaveChangesAsync();
            List = await db.ServiceManager.GetServiceList();
        }
        private async Task OnDeActive(int id)
        {
            using UnitOfWork db = new();
            await db.ServiceManager.ChangeStatus(id, false);
            await db.SaveChangesAsync();
            List = await db.ServiceManager.GetServiceList();
        }

        public static implicit operator ServiceViewModel(UnitViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}
