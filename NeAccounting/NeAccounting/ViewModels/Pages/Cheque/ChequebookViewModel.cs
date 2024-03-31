using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using NeAccounting.Views.Pages.Cheque;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class ChequebookViewModel : ObservableObject, INavigationAware
    {
        private bool _isInit;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;

        public ChequebookViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
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
            var t = await db.DocumentManager.GetChequeByDate(null, null, CusId, Status, _isInit, CurrentPage);
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
            var t = await db.DocumentManager.GetChequeByDate(StartDate, EndDate, CusId, Status, _isInit, CurrentPage);
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
            if (!CusId.HasValue)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (StartDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ شروع"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (EndDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پایان"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            using UnitOfWork db = new();
            var t = await db.DocumentManager.GetChequeByDate(StartDate, EndDate, CusId, Status, _isInit, CurrentPage);
            InvList = t.Items;
            PageCount = t.PageCount;
        }

        [RelayCommand]
        private async Task OnRemoveDoc(Guid parameter)
        {
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
                if (s)
                {
                    await db.SaveChangesAsync();
                    _snackbarService.Show("کاربر گرامی", $"حذف چک با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
                    var t = await db.DocumentManager.GetChequeByDate(StartDate, EndDate, CusId, Status, _isInit, CurrentPage);
                    InvList = t.Items;
                    PageCount = t.PageCount;
                    return;
                }

                _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            }
        }

        [RelayCommand]
        private async Task OnUpdateDoc(Guid parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("UpdateCheque");

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
                CusId = i.CustomerId,
                Substatus = i.SubmitStatus,
                SubmitDate = i.SubmitDate,
                Accunt_Number = i.Accunt_Number,
                Bank_Branch = i.Bank_Branch,
                Bank_Name = i.Bank_Name,
                Cheque_Number = i.Cheque_Number,
                CusName = i.CusName,
                Cuslist = await db.CustomerManager.GetDisplayUser(),
                Cheque_Owner = i.Cheque_Owner,
                CusNum = i.CusNum,
                Description = i.Descripion,
                DocId = i.Id,
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
        private async Task OnTransfer(Guid parameter)
        {
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

            var context = new TransferChequePage(new TransferChequeViewModel(_snackbarService, _navigationService)
            {
                Substatus = i.SubmitStatus,
                Accunt_Number = i.Accunt_Number,
                Bank_Branch = i.Bank_Branch,
                Bank_Name = i.Bank_Name,
                PayerName = i.CusName,
                Cheque_Number = i.Cheque_Number,
                Cuslist = await db.CustomerManager.GetDisplayUser(),
                Cheque_Owner = i.Cheque_Owner,
                DocId = i.Id,
                DueDate = i.DueDate,
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
                    var t = await db.DocumentManager.GetChequeByDate(StartDate, EndDate, CusId, Status, _isInit, CurrentPage);
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
                    var t = await db.DocumentManager.GetChequeByDate(StartDate, EndDate, CusId, Status, _isInit, CurrentPage);
                    InvList = t.Items;
                    PageCount = t.PageCount;
                    return;
                }

                _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            }
        }
        #endregion
    }
}




