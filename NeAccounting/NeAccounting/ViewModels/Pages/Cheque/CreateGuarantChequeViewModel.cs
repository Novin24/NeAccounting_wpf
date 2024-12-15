using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;
public partial class CreateGuarantChequeViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject, INavigationAware
{
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly INavigationService _navigationService = navigationService;
    private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;


    #region Properties

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

    public string CusName { get; set; }
    public string CusNum { get; set; }

    [ObservableProperty]
    private DateTime? _submitDate = DateTime.Now;

    /// <summary>
    /// تاریخ سررسید
    /// </summary>
    [ObservableProperty]
    private DateTime? _dueDate = DateTime.Now;

    /// <summary>
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
	/// سریال چک
	/// </summary>
	[ObservableProperty]
	private string? _cheque_Number;

	/// <summary>
	/// سری چک
	/// </summary>
	[ObservableProperty]
	private string? _cheque_Series;

	/// <summary>
	/// شماره صیادی
	/// </summary>
	[ObservableProperty]
	private string? _siadyNumber;

	/// <summary>
	/// شماره شبا
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
    /// نوع سند 
    /// </summary>
    [ObservableProperty]
    private SubmitChequeStatus _status = SubmitChequeStatus.Register;
    #endregion

    #region Method
    public async void OnNavigatedTo()
    {
        await InitializeViewModel();
    }

    private async Task InitializeViewModel()
    {
        using UnitOfWork db = new();
        Cuslist = await db.CustomerManager.GetDisplayUser();
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
        if (_isreadonly)
        {
            _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
            return;
        }
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

        if (Status != SubmitChequeStatus.NoNeedRegister && DueDate == null)
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
            Description = $"چک ({Cheque_Number}) دریافتی از مشتری";
        }

        #endregion

        #region CreatePayDocumetn
        using UnitOfWork db = new();
        var (e, s) = await db.DocumentManager.CreateGarantyCheque(CusId.Value, Status, Description, SubmitDate.Value, DueDate, Price.Value, Cheque_Number, Cheque_Series, SiadyNumber, Accunt_Number, Bank_Name, Bank_Branch, Cheque_Owner);
        if (s)
        {
            await db.SaveChangesAsync();
            _snackbarService.Show("کاربر گرامی", $"ثبت چک با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

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
    #endregion
}
