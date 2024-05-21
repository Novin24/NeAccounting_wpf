using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.Extension;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Globalization;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;
namespace NeAccounting.ViewModels
{
    public partial class CreateNotifViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject
    {
        private readonly ISnackbarService _snackbarService = snackbarService;
        private readonly INavigationService _navigationService = navigationService;
        private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;

        /// <summary>
        /// توضیحات
        /// </summary>
        [ObservableProperty]
        private string _message;

        /// <summary>
        /// عنوان
        /// </summary>
        [ObservableProperty]
        private string _titele;

        /// <summary>
        /// تاریخ سررسید
        /// </summary>
        [ObservableProperty]
        private DateTime? _dueDate = DateTime.Now;


        /// <summary>
        /// درجه اهمیت
        /// </summary>
        [ObservableProperty]
        private Priority _priority = Priority.Low;


        /// <summary>
        /// ثبت فاکتور
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnSubmit()
        {
            #region validation

            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(Titele))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("عنوان یادآور"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(Message))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("توضیحات"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (DueDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ سررسید"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            #endregion

            #region CreateNotif
            using BaseUnitOfWork baseDb = new();
            var (er, i) = await baseDb.NotifRepository.CreateNotif(Titele, Message, Priority, DueDate.Value);
            if (!i)
            {
                _snackbarService.Show("خطا", er, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            _snackbarService.Show("کاربر گرامی", $"ثبت یادآور با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            #endregion

            #region redirect
            Type? pageType = NameToPageTypeConverter.Convert("NotificationList");
            if (pageType == null)
            {
                return;
            }
            _navigationService.Navigate(pageType);
            return;
            #endregion

        }
    }
}