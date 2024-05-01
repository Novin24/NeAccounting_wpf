using DomainShared.Constants;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;
public partial class CreateFinancialViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject
{
    private bool _isreadonly = NeAccountingConstants.ReadOnlyMode;
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly INavigationService _navigationService = navigationService;

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
        using UnitOfWork nDb = new();
        var users = nDb.CustomerManager.GetCustomerList(string.Empty, string.Empty, string.Empty);
        var units = nDb.UnitManager.GetUnitList();
        var mat = nDb.MaterialManager.GetMaterails(string.Empty, string.Empty);


        #region CreatePayDocumetn
        using BaseUnitOfWork db = new();
        var g = Guid.NewGuid().ToString().Replace("-", "")[15..];
        var s = await db.FinancialYearRepository.CreateNewFinancialYear(Titele, g, Description);
        if (!s)
        {
            await db.SaveChangesAsync();
            _snackbarService.Show("کاربر گرامی", $"افتتاح سال مالی جدید با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

            Type? pageType = NameToPageTypeConverter.Convert("Dashboard");

            if (pageType == null)
            {
                return;
            }

            _navigationService.Navigate(pageType);
            return;
        }

        _snackbarService.Show("خطا", "خطا در افتتاح سال مالی جدید!!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        #endregion
    }
    #endregion
}
