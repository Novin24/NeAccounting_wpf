using DomainShared.Errore;
using DomainShared.Extension;
using Infrastructure.UnitOfWork;
using NeAccounting.Models;
using Stimulsoft.Base.Localization;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;
public partial class BackupViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject, INavigationAware
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
    private ObservableCollection<BackFilesDetails> _bakFiles = [];

    /// <summary>
    /// نام فایل
    /// </summary>
    [ObservableProperty]
    private string _fileName;

    public void OnNavigatedFrom()
    {
    }

    public void OnNavigatedTo()
    {
        FileName = SetName();
        BindFiles();
    }
    /// <summary>
    /// ثبت فاکتور
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private void OnSubmit()
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
        var (s, e) = db.BackUpRepository.GetBackup(local, ExPaht + "\\" + FileName);
        if (s)
        {
            _snackbarService.Show("کاربر گرامی", $"پشتیبان گیری با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            FileName = SetName();
            BindFiles();
            return;
        }

        _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(8000));
        #endregion
    }

    [RelayCommand]
    private void OnRestor(Guid parameter)
    {
        var file = BakFiles.First(x => x.Id == parameter).FullName;
        using BaseUnitOfWork db = new();
        var (s, e) = db.BackUpRepository.Restore(file);
        if (s)
        {
            _snackbarService.Show("کاربر گرامی", $"بازیابی اطلاعات با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            FileName = SetName();
            BindFiles();
            return;
        }

        _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(8000));
    }

    private static string SetName()
    {
        return "BackUpDb_" + Guid.NewGuid().ToString().Replace("-", "")[..15] + "&" + ".bak";
    }

    public void BindFiles()
    {
        PersianCalendar pc = new();
        try
        {
            List<BackFilesDetails> myFiles = [];
            DirectoryInfo d = new(Environment.CurrentDirectory + @"\BackUp\");
            FileInfo[] Files = d.GetFiles("*.bak");
            foreach (FileInfo file in Files)
            {
                BackFilesDetails Myfile = new()
                {
                    Id = Guid.NewGuid(),
                    FileName = file.Name,
                    FullName = file.FullName,
                    FulePath = file.DirectoryName,
                    CreationDate = file.CreationTime.ToShamsiDate(pc),
                    CreationTime = file.CreationTime.ToString("HH:mm:ss")
                };
                myFiles.Add(Myfile);
            }

            if (!string.IsNullOrEmpty(ExPaht))
            {
                DirectoryInfo Md = new($@"{ExPaht}");
                FileInfo[] MFiles = Md.GetFiles("*.bak");
                foreach (FileInfo file in MFiles)
                {
                    BackFilesDetails nMyfile = new()
                    {
                        Id = Guid.NewGuid(),
                        FileName = file.Name,
                        FulePath = file.DirectoryName,
                        FullName = file.FullName,
                        CreationTime = file.CreationTime.ToString("HH:mm:ss"),
                        CreationDate = file.CreationTime.ToShamsiDate(pc),
                    };
                    myFiles.Add(nMyfile);
                }
            }
            myFiles = myFiles.OrderBy(f => f.CreationDate).ThenBy(f => f.CreationTime).ToList();

            BakFiles = new ObservableCollection<BackFilesDetails>(myFiles);
        }
        catch (Exception ex)
        {
            _snackbarService.Show("خطا", ex.Message, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(8000));
        }
    }
}