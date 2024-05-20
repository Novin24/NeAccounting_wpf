using DomainShared.Constants;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Resources;
using NeAccounting.Windows;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;
public partial class CreateFinancialViewModel(ISnackbarService snackbarService, WindowsProviderService serviceProvider) : ObservableObject
{
    private bool _isreadonly = NeAccountingConstants.ReadOnlyMode;
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly WindowsProviderService _windowsProviderService = serviceProvider;


    #region Properties

    /// <summary>
    /// <summary>
    /// عنوان
    /// </summary>
    [ObservableProperty]
    private string _titele;

    /// <summary>
    /// توضیحات 
    /// </summary>
    [ObservableProperty]
    private string _description;
    #endregion

    #region Method   
    /// <summary>
    /// ثبت فاکتور
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task OnSubmit()
    {
        #region validation


        if (string.IsNullOrEmpty(Titele))
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("عنوان"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (_isreadonly)
        {
            _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
            return;
        }
        #endregion

        //var msg = new Wpf.Ui.Controls.MessageBox()
        //{
        //    Title = "اخطار !!!!",
        //    IsEnabled = false
        //};
        //await msg.ShowDialogAsync();

        //var ts = new List<Task>
        //{
        //    Task.Run(() => msg.ShowDialogAsync())
        //};

        //if (!typeof(Window).IsAssignableFrom(typeof(WatingWindow)))
        //{
        //    throw new InvalidOperationException(
        //        $"The window class should be derived from {typeof(Window)}."
        //    );
        //}

        using BaseUnitOfWork db = new();
        if (await db.FinancialYearRepository.CheckNameExist(Titele))
        {
            _snackbarService.Show("خطا", "عنوان وارد شده تکراری میباشد \n لطفا عنوان جدید انتخاب نمایید", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(5000));
            return;
        }
        NeAccountingConstants.NewDbTitel = Titele;
        NeAccountingConstants.NewDbDesc = Description;
        _windowsProviderService.ShowDialog<WatingWindow>();
        NeAccountingConstants.NewDbTitel = string.Empty;
        NeAccountingConstants.NewDbDesc = string.Empty;

        //var users = new List<CustomerListDto>();
        //var units = new List<UnitListDto>();
        //var mat = new List<PunListDto>();
        //var leftOver = new List<UserLeftOve>();
        //using (UnitOfWork oDb = new())
        //{
        //    users = await oDb.CustomerManager.GetCustomerList(string.Empty, string.Empty, string.Empty);
        //    units = await oDb.UnitManager.GetUnitList();
        //    mat = await oDb.MaterialManager.GetMaterails(string.Empty, string.Empty);
        //    leftOver = await oDb.DocumentManager.GetUserLeftOver();
        //}

        //#region CreatePayDocumetn
        //using BaseUnitOfWork db = new();
        //var g = Guid.NewGuid().ToString().Replace("-", "")[15..];
        //var path = Directory.GetCurrentDirectory();
        //var filePath = path + "\\" + g + ".mdf";
        //var logfilePath = path + "\\" + g + "_log.ldf";
        //var (iis, e) = await db.FinancialYearRepository.CreateNewDatabase(g, filePath, logfilePath);
        //var s = await db.FinancialYearRepository.CreateNewFinancialYear(Titele, g, Description);
        //if (!s)
        //{
        //    _snackbarService.Show("خطا", "خطا در افتتاح سال مالی جدید!!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //    return;
        //}
        //var d = await db.FinancialYearRepository.CloseLastFinancialYear();
        //if (!d)
        //{
        //    _snackbarService.Show("خطا", "خطا در ثبت سند اختتامیه!!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //    return;
        //}
        //NeAccountingConstants.NvoinDbConnectionStrint = g;


        //using (UnitOfWork nDb = new())
        //{
        //    var (er, iss) = await nDb.CustomerManager.AddAllCusInNewYear(users);
        //    var (err, isc) = await nDb.UnitManager.AddAllUnitsInNewYear(units);
        //    var (error, isScu) = await nDb.DocumentManager.AddUserLeftOver(leftOver);
        //    var (erro, iscu) = await nDb.MaterialManager.AddAllMaterialsInNewYear(mat);
        //    if (!(iss && isc && iscu))
        //    {
        //        _snackbarService.Show("خطا", "خطا در ثبت سند افتتاحیه!!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //        return;
        //    }
        //    await nDb.SaveChangesAsync();
        //}

        //await db.SaveChangesAsync();

        //_snackbarService.Show("کاربر گرامی", $"افتتاح سال مالی جدید با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

        //Type? pageType = NameToPageTypeConverter.Convert("Dashboard");

        //if (pageType == null)
        //{
        //    return;
        //}

        //_navigationService.Navigate(pageType);
        //return;

        //#endregion
    }
    #endregion
}
