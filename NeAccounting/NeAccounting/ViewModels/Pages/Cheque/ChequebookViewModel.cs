using Domain.NovinEntity.Cheques;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.Extension;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Models;
using NeAccounting.Views.Pages;
using NeApplication.Services;
using System.Globalization;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

public partial class ChequebookViewModel : ObservableObject, INavigationAware
{
	private bool _isInit;
	private readonly INavigationService _navigationService;
	private readonly IContentDialogService _contentDialogService;
	private readonly ISnackbarService _snackbarService;
	private readonly IPrintServices _printServices;
	private bool _isreadonly = true;

	public ChequebookViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService, IPrintServices printServices)
	{
		_navigationService = navigationService;
		_contentDialogService = contentDialogService;
		_printServices = printServices;
		_snackbarService = snackbarService; _isreadonly = NeAccountingConstants.ReadOnlyMode;

	}

	#region Properties

	[ObservableProperty]
	private long? _personelId;

	[ObservableProperty]
	private int _pageCount = 1;

	[ObservableProperty]
	private int _currentPage = 1;

	[ObservableProperty]
	private Guid? _cusId;

	[ObservableProperty]
	private string _chequeNumber;

	[ObservableProperty]
	private DateTime? _startDate;

	[ObservableProperty]
	private DateTime? _endDate;

	/// <summary>
	/// Enum Id 
	/// </summary>
	[ObservableProperty]
	private ChequeStatus _status = ChequeStatus.AllCheques;

	/// <summary>
	/// لیست مشتری ها
	/// </summary>
	[ObservableProperty]
	private List<SuggestBoxViewModel<Guid, long>> _cuslist;

	/// <summary>
	/// لیست چک
	/// </summary>
	[ObservableProperty]
	private IEnumerable<ChequeListDtos> _invList;
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
		_isInit = true;
		using UnitOfWork db = new();
		var t = await db.DocumentManager.GetChequeByDate(null, null, CusId, ChequeNumber, Status, _isInit, CurrentPage);
		CurrentPage = t.CurrentPage;
		InvList = t.Items;
		PageCount = t.PageCount;
		Cuslist = await db.CustomerManager.GetDisplayUser(true);
		_isInit = false;
	}

	[RelayCommand]
	private async Task OnSearch()
	{
		_isInit = true;
		using UnitOfWork db = new();
		var t = await db.DocumentManager.GetChequeByDate(StartDate, EndDate, CusId, ChequeNumber, Status, _isInit, CurrentPage);
		CurrentPage = t.CurrentPage;
		InvList = t.Items;
		PageCount = t.PageCount;
		_isInit = false;
	}

	[RelayCommand]
	private async Task OnChangePage()
	{
		if (_isInit)
		{
			return;
		}
		//if (!CusId.HasValue)
		//{
		//    _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
		//    return;
		//}
		//if (StartDate == null)
		//{
		//    _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ شروع"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
		//    return;
		//}

		//if (EndDate == null)
		//{
		//    _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پایان"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
		//    return;
		//}
		using UnitOfWork db = new();
		var t = await db.DocumentManager.GetChequeByDate(StartDate, EndDate, CusId, ChequeNumber, Status, _isInit, CurrentPage);
		InvList = t.Items;
		PageCount = t.PageCount;
	}

	[RelayCommand]
	private async Task OnRemoveDoc(Guid parameter)
	{
		if (_isreadonly)
		{
			_snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
			return;
		}
		var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
		{
			Title = "هشدار !!!",
			Content = new TextBlock() { Text = "آیا از حذف چک اطمینان دارید ؟", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
			PrimaryButtonText = "بله",
			SecondaryButtonText = "خیر",
			CloseButtonText = "انصراف",
		});
		if (result == ContentDialogResult.Primary)
		{
			using UnitOfWork db = new();
			var (e, s) = await db.DocumentManager.RemoveCheque(parameter);
			if (!s)
			{
				_snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
				return;
			}
			await db.SaveChangesAsync();
			_snackbarService.Show("کاربر گرامی", $"حذف چک با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
			var t = await db.DocumentManager.GetChequeByDate(StartDate, EndDate, CusId, ChequeNumber, Status, _isInit, CurrentPage);
			InvList = t.Items;
			PageCount = t.PageCount;

			using BaseUnitOfWork baseDb = new();
			await baseDb.NotifRepository.DeleteNotif(parameter);
		}
	}

	[RelayCommand]
	private async Task OnUpdateDoc(Guid parameter)
	{
		if (_isreadonly)
		{
			_snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
			return;
		}
		using UnitOfWork db = new();

		var (s, i) = await db.DocumentManager.GetChequeDetailById(parameter);

		var users = await db.CustomerManager.GetDisplayUser();
		var payer = users.FirstOrDefault(t => t.Id == i.PayerId);
		if (payer != null)
		{
			users.Remove(payer);
		}
		if (i.Status == ChequeStatus.Transferred)
		{
			Type? pagType = NameToPageTypeConverter.Convert("UpdateTransferCheque");

			if (pagType == null)
			{
				return;
			}
			PersianCalendar pc = new();
			var cntx = new UpdateTransferChequePage(new UpdateTransferChequeViewModel(_snackbarService, _navigationService)
			{
				CusId = i.ReceverId,
				Substatus = i.SubmitStatus,
				SubmitDate = i.TransferDate,
				Shaba_Number = i.Shaba_Number,
				Bank_Branch = i.Bank_Branch,
				Bank_Name = i.Bank_Name,
				PayerName = i.PayCusName,
				Cheque_Number = i.Cheque_Number,
				Cheque_Series = i.Cheque_Series,
				SiadyNumber = i.SiadyNumber,
				CusName = i.RecCusName,
				Cuslist = users,
				Cheque_Owner = i.Cheque_Owner,
				CusNum = i.RecCusNum,
				Description = i.RecDescripion,
				DocId = parameter,
				DueDate = i.DueDate.ToShamsiDate(pc),
				Price = i.Price,
				EnumSource = SubmitChequeStatus.Register.ToEnumDictionary()
			});

			var servis = _navigationService.GetNavigationControl();
			servis.Navigate(pagType, cntx);
			return;
		}

		Type? pageType = NameToPageTypeConverter.Convert("UpdateCheque");

		if (pageType == null)
		{
			return;
		}
		string pageName;
		if (i.Status == ChequeStatus.Payed)
		{
			pageName = "ویرایش چک پرداختی";
		}
		else if (i.Status == ChequeStatus.Guarantee)
		{
			pageName = "ویرایش چک ضمانتی";
		}
		else
		{
			pageName = "ویرایش چک دریافتی";
		}
		var context = new UpdateChequePage(new UpdateChequeViewModel(_snackbarService, _navigationService)
		{
			CusId = i.PayerId,
			Substatus = i.SubmitStatus,
			SubmitDate = i.SubmitDate,
			Shaba_Number = i.Shaba_Number,
			Bank_Branch = i.Bank_Branch,
			Bank_Name = i.Bank_Name,
			Cheque_Number = i.Cheque_Number,
			Cheque_Series = i.Cheque_Series,
			SiadyNumber = i.SiadyNumber,
			CusName = (i.Status == ChequeStatus.Payed) ? i.RecCusName : i.PayCusName,
			Cuslist = await db.CustomerManager.GetDisplayUser(),
			Cheque_Owner = i.Cheque_Owner,
			CusNum = (i.Status == ChequeStatus.Payed) ? i.RecCusNum : i.PayCusNum,
			Description = i.PayDescripion,
			DocId = parameter,
			DueDate = i.DueDate,
			Status = i.Status,
			PageName = pageName,
			Price = i.Price,
			EnumSource = SubmitChequeStatus.Register.ToEnumDictionary()
		});

		var servise = _navigationService.GetNavigationControl();
		servise.Navigate(pageType, context);
	}

	[RelayCommand]
	private async Task OnDetailsDoc(Guid parameter)
	{
		Type? pageType = NameToPageTypeConverter.Convert("CheckDetails");

		if (pageType == null)
		{
			return;
		}
		using UnitOfWork db = new();
		var (s, i) = await db.DocumentManager.GetChequeDetailById(parameter);
		if (!s)
		{
			_snackbarService.Show("کاربر گرامی", $"چک مورد نظر یافت نشد!!!", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
			return;
		}
		PersianCalendar pc = new();
		var context = new CheckDetailsPage(new DetailsChequeViewModel(_navigationService)
		{
			SubStatus = i.SubmitStatus.ToDisplay(),
			SubmitDate = i.SubmitDate.ToShamsiDate(pc),
			TransferDate = i.TransferDate.ToShamsiDate(pc),
			DueDate = i.DueDate.ToShamsiDate(pc),
			Shaba_Number = i.Shaba_Number,
			Bank_Branch = i.Bank_Branch,
			Bank_Name = i.Bank_Name,
			Cheque_Number = i.Cheque_Number,
			Cheque_Series = i.Cheque_Series,
			SiadyNumber = i.SiadyNumber,
			PayCusName = i.PayCusName,
			PayCusNum = i.PayCusNum,
			CusName = i.RecCusName,
			CusNum = i.RecCusNum,
			Cheque_Owner = i.Cheque_Owner,
			PayDescription = i.PayDescripion,
			RecDescription = i.RecDescripion,
			Price = i.Price.ToString("N0"),
			Status = i.Status,
		});

		var servise = _navigationService.GetNavigationControl();
		servise.Navigate(pageType, context);
	}

	public async Task<(bool isSuccess, ChequeForPrintDto cheque)> PrintCheque(Guid chequeId)
	{
		using UnitOfWork db = new();
		var (isSuccess, cheque) = await db.DocumentManager.GetChequeForPrint(chequeId);

		return (true, cheque);
	}

	[RelayCommand]
	private async Task OnPrintCheque(Guid parameter)
	{
		var (isSuccess, chequeInfo) = await PrintCheque(parameter);


		PersianCalendar pc = new();
		Dictionary<string, string> dic = new()
		{
			{"Cheque_Number",$"{chequeInfo.Cheque_Number}"},
			{"Cheque_Series",$"{chequeInfo.Cheque_Series}"},
			{"SiadyNumber",$"{chequeInfo.SiadyNumber}"},
			{"Bank_Name",$"{chequeInfo.Bank_Name}"},
			{"Bank_Branch",$"{chequeInfo.Bank_Branch}"},
			{"Price",$"{chequeInfo.Price}"},
			{"Mines",$"{chequeInfo.Mines}"},
			{"StringPrice",$"{chequeInfo.stringPrice}"},
			{"Cheque_Owner",$"{chequeInfo.Cheque_Owner}"},
			{"CusName",$"{chequeInfo.RecCusName}"},
			{"DueShamsiDate",$"{chequeInfo.DueShamsiDate}"},
			{"StingShamsiDate",$"{chequeInfo.StingShamsiDate}"},
			{"Accunt_Number",$"{chequeInfo.Shaba_Number}"},
			{"NationalCode",$"{chequeInfo.NationalCode}"},
		};

		_printServices.PrintInvoice(@"Required\Reports\ChequeReport.mrt", dic);
	}

	[RelayCommand]
	private async Task OnTransfer(Guid parameter)
	{
		if (_isreadonly)
		{
			_snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
			return;
		}
		Type? pageType = NameToPageTypeConverter.Convert("TransferCheque");

		if (pageType == null)
		{
			return;
		}

		using UnitOfWork db = new();
		var (s, i) = await db.DocumentManager.GetChequeById(parameter);
		if (!s)
		{
			_snackbarService.Show("کاربر گرامی", $"چک مورد نظر یافت نشد!!!", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
			return;
		}

		var users = await db.CustomerManager.GetDisplayUser();
		var payer = users.FirstOrDefault(t => t.Id == i.CustomerId);
		if (payer != null)
		{
			users.Remove(payer);
		}

		PersianCalendar pc = new();
		var context = new TransferChequePage(new TransferChequeViewModel(_snackbarService, _navigationService)
		{
			Substatus = i.SubmitStatus,
			Shaba_Number = i.Shaba_Number,
			Bank_Branch = i.Bank_Branch,
			Bank_Name = i.Bank_Name,
			PayerName = i.CusName,
			Cheque_Number = i.Cheque_Number,
			Cheque_Series = i.Cheque_Series,
			SiadyNumber = i.SiadyNumber,
			Cuslist = users,
			Cheque_Owner = i.Cheque_Owner,
			DocId = i.Id,
			DueDate = i.DueDate.ToShamsiDate(pc),
			Price = i.Price,
			EnumSource = SubmitChequeStatus.Register.ToEnumDictionary()
		});

		var servise = _navigationService.GetNavigationControl();
		servise.Navigate(pageType, context);
	}

	[RelayCommand]
	private void OnAddClick(string parameter)
	{
		if (string.IsNullOrWhiteSpace(parameter))
		{
			return;
		}

		Type? pageType = NameToPageTypeConverter.Convert(parameter);

		if (pageType == null)
		{
			return;
		}

		_ = _navigationService.Navigate(pageType);
	}

	[RelayCommand]
	private async Task OnConvertToCash(Guid parameter)
	{
		if (_isreadonly)
		{
			_snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
			return;
		}
		var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
		{
			Title = "هشدار !!!",
			Content = new TextBlock() { Text = "آیا از نقد کردن چک اطمینان دارید ؟", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
			PrimaryButtonText = "بله",
			SecondaryButtonText = "خیر",
			CloseButtonText = "انصراف",
		});
		if (result == ContentDialogResult.Primary)
		{
			using UnitOfWork db = new();
			var (e, s) = await db.DocumentManager.ConvertChequeToCash(parameter);
			if (s)
			{
				await db.SaveChangesAsync();
				_snackbarService.Show("کاربر گرامی", $"نقد چک با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
				var t = await db.DocumentManager.GetChequeByDate(StartDate, EndDate, CusId, ChequeNumber, Status, _isInit, CurrentPage);
				InvList = t.Items;
				PageCount = t.PageCount;
				return;
			}

			_snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
		}
	}

	[RelayCommand]
	private async Task OnRejects(Guid parameter)
	{
		if (_isreadonly)
		{
			_snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
			return;
		}
		var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
		{
			Title = "هشدار !!!",
			Content = new TextBlock() { Text = "آیا از برگشت چک اطمینان دارید ؟", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
			PrimaryButtonText = "بله",
			SecondaryButtonText = "خیر",
			CloseButtonText = "انصراف",
		});
		if (result == ContentDialogResult.Primary)
		{
			using UnitOfWork db = new();
			var (e, s) = await db.DocumentManager.ConvertChequeToReject(parameter);
			if (s)
			{
				await db.SaveChangesAsync();
				_snackbarService.Show("کاربر گرامی", $"نقد چک با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
				var t = await db.DocumentManager.GetChequeByDate(StartDate, EndDate, CusId, ChequeNumber, Status, _isInit, CurrentPage);
				InvList = t.Items;
				PageCount = t.PageCount;
				return;
			}

			_snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
		}
	}
	#endregion
}




