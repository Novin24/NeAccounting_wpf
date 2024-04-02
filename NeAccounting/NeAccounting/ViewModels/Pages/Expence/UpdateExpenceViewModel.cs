using DomainShared.Enums;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class UpdateExpenceViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;

        public UpdateExpenceViewModel(ISnackbarService snackbarService, INavigationService navigationService)
        {
            _snackbarService = snackbarService;
            _navigationService = navigationService;
        }
        /// <summary>
        /// تایپ پرداخت 
        /// </summary>
        [ObservableProperty]
        private Dictionary<Enum, string> _payTypeEnum;

        /// <summary>
        /// تاریخ ثبت 
        /// </summary>
        [ObservableProperty]
        private DateTime? _submitDate;

        /// <summary>
        /// عنوان هزینه 
        /// </summary>
        [ObservableProperty]
        private string _expensetype;

        /// <summary>
        /// قیمت 
        /// </summary>
        [ObservableProperty]
        private long? _amount = 0;

        /// <summary>
        /// Enum Id 
        /// </summary>
        [ObservableProperty]
        private PaymentType _payTypeId ;

        /// <summary>
        /// دریافت کننده 
        /// </summary>
        [ObservableProperty]
        private string _receiver;

        /// <summary>
        /// توضیحات 
        /// </summary>
        [ObservableProperty]
        private string _description;

        /// <summary>
        /// Id 
        /// </summary>
        [ObservableProperty]
        private Guid _expenseID;


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
            #region validation
            if (SubmitDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(Expensetype))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نوع هزینه"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (Amount == null || Amount == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            #endregion

            #region UpdateExpense
            using UnitOfWork db = new();
            var (error, isSuccess) = await db.ExpenseManager.UpdateExpense(ExpenseID, SubmitDate.Value, Expensetype, Amount.Value, PayTypeId, Receiver, Description);
            if (!isSuccess)
            {
                await db.SaveChangesAsync();
                _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            await db.SaveChangesAsync();
            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));


            Type? pageType = NameToPageTypeConverter.Convert("ExpencesList");

            if (pageType == null)
            {
                return;
            }

            _navigationService.Navigate(pageType);
            #endregion
        }
    }
}
