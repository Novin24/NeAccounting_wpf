using DomainShared.Errore;
using DomainShared.Extension;
using Infrastructure.UnitOfWork;
using NeAccounting.Models;
using System.Globalization;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;
public partial class BackupViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject
{
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly INavigationService _navigationService = navigationService;

    /// <summary>
    /// ادرس فایل
    /// </summary>
    [ObservableProperty]
    private string _exPaht;
    
    /// <summary>
    /// لیست فایل ها
    /// </summary>
    [ObservableProperty]
    private List<BackFilesDetails> _bakFiles = [];

    /// <summary>
    /// نام فایل
    /// </summary>
    [ObservableProperty]
    private string _fileName;

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
        if (string.IsNullOrEmpty(ExPaht))
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("آدرس فایل"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }
        #endregion

        #region CreatePayDocumetn
        string local = Environment.CurrentDirectory + @"\BackUp\" + FileName;
        using BaseUnitOfWork db = new();
        var (s, e) = db.BackUpRepository.GetBackup(local, ExPaht + FileName);
        if (s)
        {
            await db.SaveChangesAsync();
            _snackbarService.Show("کاربر گرامی", $"ثبت چک با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            FileName = SetName();
            return;
        }

        _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        #endregion
    }
    private string SetName()
    {
        PersianCalendar pc = new();
        var time = DateTime.Now.ToShamsiDate(pc, '^');
        return "BackUpDb_" + Guid.NewGuid().ToString().Replace("-", "")[..15] + "&" + time + ".bak";
    }

    private async Task onRestor(Guid parameter)
    {

    }
}