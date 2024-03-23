using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;
namespace NeAccounting.ViewModels
{
    public partial class UpdateChequeViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService = snackbarService;
        private readonly INavigationService _navigationService = navigationService;


        /// <summary>
        /// لیست مشتری ها
        /// </summary>
        [ObservableProperty]
        private List<SuggestBoxViewModel<Guid, long>> _cuslist;

        /// <summary>
        /// شناسه مشتری
        /// </summary>
        [ObservableProperty]
        private Guid? _CusId;

        /// <summary>
        /// شناسه سند
        /// </summary>
        [ObservableProperty]
        private Guid _docId;
        
        /// <summary>
        /// وضعیت ثبت
        /// </summary>
        [ObservableProperty]
        private Dictionary<Enum,string> _enumSource;

        /// <summary>
        /// نام مشتری
        /// </summary>
        [ObservableProperty]
        private string _cusName;

        /// <summary>
        /// شماره مشتری
        /// </summary>
        [ObservableProperty]
        private string _cusNum;

        [ObservableProperty]
        private DateTime? _submitDate = DateTime.Now;

        /// <summary>
        /// تاریخ سررسید
        /// </summary>
        [ObservableProperty]
        private DateTime? _dueDate = DateTime.Now;

        /// <summary>
        /// نام صفحه
        /// </summary>
        [ObservableProperty]
        private string _pageName= "ویرایش";

        /// <summary>
        /// مبلغ چک 
        /// </summary>
        [ObservableProperty]
        private long? _price;

        /// <summary>
        /// توضیحات 
        /// </summary>
        [ObservableProperty]
        private string? _description;

        /// <summary>
        /// شماره چک
        /// </summary>
        [ObservableProperty]
        private string? _cheque_Number;


        /// <summary>
        /// شماره حساب
        /// </summary>
        public string Accunt_Number { get; set; }

        /// <summary>
        /// نام بانک
        /// </summary>
        public string Bank_Name { get; set; }

        /// <summary>
        /// نام شعبه
        /// </summary>
        public string Bank_Branch { get; set; }

        /// <summary>
        /// صاحب چک
        /// </summary>
        public string Cheque_Owner { get; set; }

        /// <summary>
        /// نوع ثبت سند 
        /// </summary>
        [ObservableProperty]
        private SubmitChequeStatus _substatus = SubmitChequeStatus.NotRegister;

        /// <summary>
        /// نوع سند 
        /// </summary>
        [ObservableProperty]
        private ChequeStatus _status ;

        public async void OnNavigatedTo()
        {
        }


        public void OnNavigatedFrom()
        {
        }

        /// <summary>
        /// ثبت فاکتور
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnSubmit()
        {
            #region validation
            if (CusId == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (SubmitDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (DueDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ سررسید"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(Cheque_Owner))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("صاحب چک"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(Cheque_Number))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("شماره چک"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (Price == null || Price == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ چک"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(Bank_Name))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام بانک"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(Description))
            {
                Description = $"چک ({Cheque_Number}) پرداختی به مشتری";
            }

            #endregion

            #region CreatePayDocumetn
            using UnitOfWork db = new();
            var (e, s) = await db.DocumentManager.UpdateCheque(DocId,CusId.Value, Substatus, Description, SubmitDate.Value, DueDate.Value, Price.Value, Cheque_Number, Accunt_Number, Bank_Name, Bank_Branch, Cheque_Owner);
            if (s)
            {
                await db.SaveChangesAsync();
                _snackbarService.Show("کاربر گرامی", $"ویرایش چک با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                Type? pageType = NameToPageTypeConverter.Convert("Chequebook");

                if (pageType == null)
                {
                    return;
                }

                _navigationService.Navigate(pageType);
                return;
            }

            _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            #endregion
        }
    }

}