using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Document;
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
    /// لیست مشتری ها
    /// </summary>
    [ObservableProperty]
    private List<SummaryDoc> _docList;

    /// <summary>
    /// شناسه مشتری
    /// </summary>
    [ObservableProperty]
    private Guid? _CusId;

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
    /// نوع سند 
    /// </summary>
    [ObservableProperty]
    private PaymentType _type;

    public async void OnNavigatedTo()
    {
        await InitializeViewModel();
    }

    private async Task InitializeViewModel()
    {
        using UnitOfWork db = new();
        Cuslist = await db.CustomerManager.GetDisplayUser(null, true);
        DocList = await db.DocumentManager.GetSummaryDocs(CusId, DocumntType.PayDoc);
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
        DocList = await db.DocumentManager.GetSummaryDocs(CusId, DocumntType.PayDoc);
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

        if (SubmitDate == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        if (Price == 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ وجه"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        #endregion

        #region CreatePayDocumetn
        using UnitOfWork db = new();
        var (e, s) = await db.DocumentManager.CreatePayDocument(CusId.Value, Type, Price, Discount, Description, SubmitDate.Value);
        if (s)
        {
            await db.SaveChangesAsync();
            _snackbarService.Show("کاربر گرامی", $"ثبت سند با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            Reload();
            return true;
        }

        _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        return false;
        #endregion
    }

    private void Reload()
    {
        SubmitDate = DateTime.Now;
        Description = string.Empty;
        Status = "تسویه";
        TotalPrice = "0";
        TotalPricee = 0;
        Price = 0;
        Discount = 0;
    }
}
