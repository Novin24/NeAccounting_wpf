using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.Utilities;
using DomainShared.ViewModels.Document;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

public partial class UpdatePayDocViewModel : ObservableObject, INavigationAware
{
    private bool _isInitialized = false;
    private readonly ISnackbarService _snackbarService;
    private readonly INavigationService _navigationService;

    public UpdatePayDocViewModel(ISnackbarService snackbarService, INavigationService navigationService)
    {
        _snackbarService = snackbarService;
        _navigationService = navigationService;
    }
    /// <summary>
    /// لیست اخرین اسناد
    /// </summary>
    [ObservableProperty]
    private List<SummaryDoc> _docList;

    /// <summary>
    /// شناسه مشتری
    /// </summary>
    [ObservableProperty]
    private Guid? _docId;

    /// <summary>
    ///نام مشتری
    /// </summary>
    [ObservableProperty]
    private string? _cusName;

    /// <summary>
    ///شماره مشتری
    /// </summary>
    [ObservableProperty]
    private long _cusNumber;

    [ObservableProperty]
    private DateTime? _submitDate = DateTime.Now;

    /// <summary>
    /// وضعیت مشتری
    /// </summary>
    [ObservableProperty]
    private string _status = "تسویه";

    /// <summary>
    /// مبلغ کل وضعیت
    /// </summary>
    [ObservableProperty]
    private string _totalPrice = "0";

    /// <summary>
    /// مبلغ کل وضعیت
    /// </summary>
    [ObservableProperty]
    private long _totalPricee = 0;

    /// <summary>
    /// مبلغ وارد شده 
    /// </summary>
    [ObservableProperty]
    private long _price = 0;

    /// <summary>
    /// تخفیف اعمال شده 
    /// </summary>
    [ObservableProperty]
    private long? _discount = 0;

    /// <summary>
    /// توضیحات 
    /// </summary>
    [ObservableProperty]
    private string? _description;

    /// <summary>
    /// Enum Id 
    /// </summary>
    public byte PayTypeId {  get; set; }



    public void OnNavigatedTo()
    {
        if (!_isInitialized)
            InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        _isInitialized = true;
    }

    public void OnNavigatedFrom()
    {
    }

    /// <summary>
    /// ثبت سند
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task OnSumbit()
    {
        #region validation
        //if (CusId == null)
        //{
        //    _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //    return false;
        //}

        if (SubmitDate == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (Price == 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ وجه"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (string.IsNullOrEmpty(Description))
        {
            Description = $"{((PaymentType)PayTypeId).ToDisplay()} پرداختی به مشتری";
        }

        #endregion

        #region CreatePayDocumetn
        //using UnitOfWork db = new();
        //var (e, s) = await db.DocumentManager.CreatePayDocument(CusId.Value, Type, Price, Discount, Description, SubmitDate.Value);
        //if (s)
        //{
        //    await db.SaveChangesAsync();
        //    _snackbarService.Show("کاربر گرامی", $"ثبت سند با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
        //    return true;
        //}

        //_snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        #endregion
    }
}
