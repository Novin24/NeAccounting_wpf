using DomainShared.Enums;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

public partial class BackupViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject
{
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly INavigationService _navigationService = navigationService;

    [ObservableProperty]
    private DateTime? _submitDate = DateTime.Now;

    /// <summary>
    /// تاریخ سررسید
    /// </summary>
    [ObservableProperty]
    private DateTime? _dueDate = DateTime.Now;

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
        if (SubmitDate == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }
        #endregion

        #region CreatePayDocumetn
        using BaseUnitOfWork db = new();
        var (s, e) = db.BackUpRepository.GetBackup("","");
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
}