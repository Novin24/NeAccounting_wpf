using DomainShared.Constants;
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
public partial class BackupViewModel(ISnackbarService snackbarService) : ObservableObject, INavigationAware
{
    private readonly ISnackbarService _snackbarService = snackbarService;

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


    /// <summary>
    /// حالت انتظار
    /// </summary>
    [ObservableProperty]
    private bool _wating = false;

    /// <summary>
    /// دیتاگرید 
    /// </summary>
    [ObservableProperty]
    private bool _showData = true;


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
    private async Task OnSubmit()
    {
        #region validation
        string backUpPath = "";
        if (string.IsNullOrEmpty(ExPaht))
        {
            backUpPath = Environment.CurrentDirectory + @"\Required\BackedUp\" + FileName;
        }
        else
        {
            backUpPath = ExPaht + "\\" + FileName;
        }
        #endregion

        #region CreatePayDocumetn
        using BaseUnitOfWork db = new();
        Wating = true;
        ShowData = false;
        var (s, e) = await db.BackUpRepository.GetBackup(backUpPath);
        Wating = false;
        ShowData = true;
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
    private async Task OnRestor(Guid parameter)
    {
        var file = BakFiles.First(x => x.Id == parameter);
        string dbName = file.FileName.Substring(9, 15);
        if (dbName != NeAccountingConstants.NvoinDbConnectionStrint)
        {
            _snackbarService.Show("کاربر گرامی", $"فایل پشتیبان مورد نظر مربوط به سال مالی کنونی نمی‌باشد !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            return;
        }
        using BaseUnitOfWork db = new();
        Wating = true;
        ShowData = false;
        var (s, e) = await db.BackUpRepository.Restore(file.FullName);
        Wating = false;
        ShowData = true;
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
        return "BackupDb_" + NeAccountingConstants.NvoinDbConnectionStrint + "_" + Guid.NewGuid().ToString().Replace("-", "")[..15] + ".bak";
    }

    public void BindFiles()
    {
        PersianCalendar pc = new();
        try
        {
            List<BackFilesDetails> myFiles = [];
            DirectoryInfo d = new(Environment.CurrentDirectory + @"\Required\BackedUp");
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