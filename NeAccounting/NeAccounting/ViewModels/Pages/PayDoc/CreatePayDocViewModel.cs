using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

public partial class CreatePayDocViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject, INavigationAware
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

    [ObservableProperty]
    private DateTime _submitDate = DateTime.Now;

    /// <summary>
    /// مقدار پورسانت
    /// </summary>
    [ObservableProperty]
    private double? _commission;

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
    /// شماره اخرین سند
    /// </summary>
    [ObservableProperty]
    private string _lastInvoice;

    /// <summary>
    /// مبلغ وارد شده 
    /// </summary>
    [ObservableProperty]
    private long _price = 0;

    /// <summary>
    /// مبلغ وارد شده 
    /// </summary>
    [ObservableProperty]
    private long? _discount = 0;

    /// <summary>
    /// توضیحات 
    /// </summary>
    [ObservableProperty]
    private string? _description;

    /// <summary>
    /// تسویه کامل
    /// </summary>
    [ObservableProperty]
    private bool _over;



    public async void OnNavigatedTo()
    {
        await InitializeViewModel();
    }

    private async Task InitializeViewModel()
    {
        using UnitOfWork db = new();
        Cuslist = await db.CustomerManager.GetDisplayUser(null, true);
        LastInvoice = await db.DocumentManager.GetLastDocumntNumber(DocumntType.PayDoc);
    }

    public void OnNavigatedFrom()
    {
    }

    /// <summary>
    /// انتخاب مشتری
    /// </summary>
    /// <param name="custId"></param>
    /// <returns></returns>
    internal async Task OnSelectCus(Guid custId)
    {
        using UnitOfWork db = new();
        var (am, stu) = await db.DocumentManager.GetStatus(custId);
        Status = stu;
        TotalPricee = Math.Abs(am);
        TotalPrice = Math.Abs(am).ToString("N0");
    }


    /// <summary>
    /// ثبت فاکتور
    /// </summary>
    /// <returns></returns>
    internal async Task<bool> OnSumbit()
    {
        #region validation
        if (CusId == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        #endregion

        using UnitOfWork db = new();
        //var (e, s, serial) = await db.DocumentManager.CreateSellDocument(CusId.Value, totalInvoicePrice, InvDescription, SubmitDate, false, List);
        //if (!s)
        //{
        //    _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //    return false;
        //}

        #region create_Commission_Doc
        if (Commission != null && Commission != 0)
        {
            //var (er, su, sr) = await db.DocumentManager.CreateDocument(CusId.Value, (long)(totalInvoicePrice * (Commission / 100)),
            //    DocumntType.Rec, $"{serial} پورسانت فاکتور", SubmitDate, true);

            //if (!su)
            //{
            //    _snackbarService.Show("خطا", er, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            //    return false;
            //}
        }
        await db.SaveChangesAsync();
        #endregion

        #region reload
        _snackbarService.Show("کاربر گرامی", $"ثبت فاکتور به شماره ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

        return true;
        #endregion
    }
}
