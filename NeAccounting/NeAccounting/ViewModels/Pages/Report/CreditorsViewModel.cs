using DomainShared.Extension;
using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using NeAccounting.Models;
using NeApplication.Services;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

public partial class CreditorsViewModel : ObservableObject, INavigationAware
{
	private readonly ISnackbarService _snackbarService;
	private readonly IPrintServices _printServices;
	public CreditorsViewModel(ISnackbarService snackbarService, IPrintServices printServices)
	{
		_snackbarService = snackbarService;
		_printServices = printServices;
	}

    #region Properties

    /// <summary>
    /// حداقل
    /// </summary>
    [ObservableProperty]
    private long _min = 0;

    /// <summary>
    /// حداکثر
    /// </summary>
    [ObservableProperty]
    private long _max = 0;

    /// <summary>
    /// لیست بدهکاران
    /// </summary>
    [ObservableProperty]
    private IEnumerable<CreditorsOrDebtorsReport> _creList;
    #endregion

    #region Methods
    public async void OnNavigatedTo()
    {
        await InitializeViewModel();
    }

    public void OnNavigatedFrom()
    {

    }

    private async Task InitializeViewModel()
    {
        using UnitOfWork db = new();
        CreList = await db.DocumentManager.GetcreditorsReport(Min, Max);
    }

    [RelayCommand]
    private async Task OnSearch()
    {
        using UnitOfWork db = new();
        CreList = await db.DocumentManager.GetcreditorsReport(Min, Max);
    }

	public async Task<(IEnumerable<CreditorsOrDebtorsReport> list, bool isSuccess)> PrintCreditors()
	{

		using UnitOfWork db = new();
		CreList = await db.DocumentManager.GetcreditorsReport(Min, Max);
		return new(CreList, true);

	}

	[RelayCommand]
	private async Task OnPrintCreditorsList()
	{
		var (list, isSuccess) = await PrintCreditors();
		if (!isSuccess)
			return;

		if (!list.Any())
		{
			_snackbarService.Show("خطا", "در بازه انتخابی موردی برای نمایش یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
			return;
		}
		var printInfo = JsonConvert.DeserializeObject<PrintInfo>(File.ReadAllText(@"Required\Reports\PrintInfo.json"));
		if (printInfo == null)
		{
			_snackbarService.Show("خطا", "فایل پرینت یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
			return;
		}

		PersianCalendar pc = new();
		Dictionary<string, string> dic = new()
		{


			{"PrintTime",DateTime.Now.ToShamsiDate(new PersianCalendar()) },
			{"Management",$"{printInfo.Management}"},
			{"Company_Name",$"{printInfo.Company_Name}"},
			{"Tabligh",$"{printInfo.Tabligh}"},
		};
		_printServices.PrintInvoice(@"Required\Reports\CreditorsReport.mrt", "CreditorsListDtos", list, dic);
	}
	#endregion
}




