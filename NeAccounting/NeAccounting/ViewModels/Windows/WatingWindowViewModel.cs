using DomainShared.Constants;
using DomainShared.ViewModels.Customer;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.Pun;
using DomainShared.ViewModels.unit;
using Infrastructure.UnitOfWork;
using System.IO;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

public partial class WatingWindowViewModel : ObservableObject
{
    private readonly ISnackbarService _snackbarService;

    #region Properties

    /// <summary>
    /// عنوان
    /// </summary>
    [ObservableProperty]
    private string _titele;

    /// <summary>
    /// متن  اخطار
    /// </summary>
    [ObservableProperty]
    private string _content = "کاربر گرامی \n عملیات تغییر سال مالی ممکن است چند دقیقه به طول بیانجامد در طی فرایند به هیچ عنوان سیستم خود را خاموش نفرمایید.";

    /// <summary>
    /// توضیحات 
    /// </summary>
    [ObservableProperty]
    private string _description;

    public WatingWindowViewModel(ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
    }
    #endregion

    #region Method
    /// <summary>
    /// عملیات سال مالی
    /// </summary>
    [RelayCommand]
    private async Task OnChangeYear()
    {
        await Task.Delay(3000);
        if (string.IsNullOrEmpty(Titele))
        {
            return;
        }
        var users = new List<CustomerListDto>();
        var units = new List<UnitListDto>();
        var mat = new List<PunListDto>();
        var leftOver = new List<UserLeftOve>();
        using (UnitOfWork oDb = new())
        {
            users = await oDb.CustomerManager.GetCustomerList(string.Empty, string.Empty, string.Empty);
            units = await oDb.UnitManager.GetUnitList();
            mat = await oDb.MaterialManager.GetMaterails(string.Empty, string.Empty);
            leftOver = await oDb.DocumentManager.GetUserLeftOver();
        }

        #region ChangeYear
        using BaseUnitOfWork db = new();
        var g = Guid.NewGuid().ToString().Replace("-", "")[15..];
        var path = Directory.GetCurrentDirectory();
        var filePath = path + "\\" + g + ".mdf";
        var logfilePath = path + "\\" + g + "_log.ldf";
        var (iis, e) = await db.FinancialYearRepository.CreateNewDatabase(g, filePath, logfilePath);
        var s = await db.FinancialYearRepository.CreateNewFinancialYear(Titele, g, Description);
        if (!s)
        {
            _snackbarService.Show("خطا", "خطا در افتتاح سال مالی جدید!!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }
        var d = await db.FinancialYearRepository.CloseLastFinancialYear();
        if (!d)
        {
            _snackbarService.Show("خطا", "خطا در ثبت سند اختتامیه!!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }
        NeAccountingConstants.NvoinDbConnectionStrint = g;


        using (UnitOfWork nDb = new())
        {
            var (er, iss) = await nDb.CustomerManager.AddAllCusInNewYear(users);
            var (err, isc) = await nDb.UnitManager.AddAllUnitsInNewYear(units);
            var (error, isScu) = await nDb.DocumentManager.AddUserLeftOver(leftOver);
            var (erro, iscu) = await nDb.MaterialManager.AddAllMaterialsInNewYear(mat);
            if (!(iss && isc && iscu))
            {
                _snackbarService.Show("خطا", "خطا در ثبت سند افتتاحیه!!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            await nDb.SaveChangesAsync();
        }

        await db.SaveChangesAsync();

        _snackbarService.Show("کاربر گرامی", $"افتتاح سال مالی جدید با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
        #endregion
    }
    #endregion
}
