using DomainShared.Constants;
using DomainShared.Errore;
using DomainShared.ViewModels.Customer;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;


namespace NeAccounting.ViewModels
{
    public partial class UpdateSellInvoiceViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private bool _isreadonly = true;

        public UpdateSellInvoiceViewModel(ISnackbarService snackbarService, INavigationService navigationService)
        {
            _snackbarService = snackbarService;
            _navigationService = navigationService;
            _isreadonly = NeAccountingConstants.ReadOnlyMode;
        }
        #region properties

        private int RowId = 1;

        [ObservableProperty]
        private Guid _invoiceId;

        /// <summary>
        /// لیست اجناس  فاکتور
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<RemittanceListViewModel> _list = [];

        /// <summary>
        /// لیست ثابت اجناس  فاکتور
        /// </summary>
        public List<RemittanceListViewModel> StaticList = [];

        /// <summary>
        /// لیست کلیه اجناس
        /// </summary>
        [ObservableProperty]
        private List<MatListDto> _matList;

        /// <summary>
        /// نام مشتری
        /// </summary>
        [ObservableProperty]
        private string _cusName;

        /// <summary>
        /// شماره مشتری
        /// </summary>
        [ObservableProperty]
        private long _cusNumber;

        [ObservableProperty]
        private DateTime? _submitDate = DateTime.Now;

        /// <summary>
        /// مقدار پورسانت
        /// </summary>
        [ObservableProperty]
        private double? _commission;

        /// <summary>
        /// وضعیت مشتری
        /// </summary>
        [ObservableProperty]
        private string _status = "تسویه";

        /// <summary>
        /// بدهکاری مشتری
        /// </summary>
        [ObservableProperty]
        private string _debt = "0";

        /// <summary>
        /// طلبکاری مشتری
        /// </summary>
        [ObservableProperty]
        private string _credit = "0";

        /// <summary>
        /// مبلغ کل فاکتور
        /// </summary>
        [ObservableProperty]
        private string _totalPrice = "0";

        /// <summary>
        /// مبلغ کل پورسانت
        /// </summary>
        [ObservableProperty]
        private string _totalcommission = "0";

        /// <summary>
        /// مبلغ باقی مانده
        /// </summary>
        [ObservableProperty]
        private string _remainPrice = "0";

        /// <summary>
        /// شماره اخرین فاکتور
        /// </summary>
        [ObservableProperty]
        private string _lastInvoice;

        /// <summary>
        /// شناسه جنس انتخاب شده در سلکت باکس
        /// </summary>
        [ObservableProperty]
        private Guid? _materialId = null;

        /// <summary>
        /// مقدار انتخاب شده
        /// </summary>
        [ObservableProperty]
        private double? _amountOf;

        /// <summary>
        /// ردیف انتخاب شده
        /// </summary>
        [ObservableProperty]
        private Guid? _remId;

        /// <summary>
        /// مبلغ انتخابی 
        /// </summary>
        [ObservableProperty]
        private long? _matPrice;

        /// <summary>
        /// توضیحات ردیف
        /// </summary>
        [ObservableProperty]
        private string? _description;

        /// <summary>
        /// توضیحات فاکتور
        /// </summary>
        [ObservableProperty]
        private string? _invDescription;
        #endregion

        #region Commands
        public async void OnNavigatedTo()
        {
            InvoiceId = EditInvoiceDetails.InvoiceId;
            using UnitOfWork db = new();
            var (isSuccess, itm) = await db.DocumentManager.GetSellInvoiceDetail(InvoiceId);
            if (!isSuccess)
            {
                _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                Type? pageType = NameToPageTypeConverter.Convert("Dashboard");

                if (pageType == null)
                {
                    return;
                }

                _navigationService.Navigate(pageType);
                return;
            }
            MatList = await db.MaterialManager.GetMaterails();
            var stu = await db.DocumentManager.GetStatus(itm.CustomerId);
            (string error, CustomerListDto cus) = await db.CustomerManager.GetCustomerById(itm.CustomerId);
            foreach (var it in itm.RemList)
            {
                List.Add(it);
            }
            CusName = cus.Name;
            CusNumber = cus.UniqNumber;
            Status = stu.Status;
            Debt = stu.Amount;
            Credit = cus.TotalCredit;
            SubmitDate = itm.Date;
            StaticList = itm.RemList;
            InvDescription = itm.InvoiceDescription;
            Commission = itm.Commission;
            LastInvoice = itm.Serial;
            TotalPrice = itm.TotalPrice.ToString("N0");
            Totalcommission = itm.Commission.HasValue ? (itm.TotalPrice * (itm.Commission.Value / 100)).ToString("N0") : "0";
            RemainPrice = itm.TotalPrice.ToString("N0");
            if (itm.CommissionPrice.HasValue && itm.CommissionPrice.Value != 0)
            {
                RemainPrice = (itm.TotalPrice - itm.CommissionPrice.Value).ToString("N0");
            }
        }

        public void OnNavigatedFrom()
        {
            EditInvoiceDetails.InvoiceId = Guid.Empty;
        }

        /// <summary>
        /// افزودن ردیف
        /// </summary>
        /// <returns></returns>
        internal bool OnAdd()
        {
            #region validation

            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return false;
            }
            if (MaterialId == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return false;
            }

            if (AmountOf == null || AmountOf <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مقدار"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return false;
            }

            if (MatPrice == null || MatPrice == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return false;
            }
            #endregion

            var mat = MatList.First(t => t.Id == MaterialId);
            List.Add(new RemittanceListViewModel()
            {
                AmountOf = AmountOf.Value,
                UnitName = mat.UnitName,
                IsService = mat.IsService,
                MatName = mat.MaterialName,
                Price = MatPrice.Value,
                RremId = RemId ?? Guid.Empty,
                RowId = RowId,
                IsDeleted = false,
                TotalPrice = (long)(MatPrice.Value * AmountOf.Value),
                Description = Description,
                MaterialId = MaterialId.Value,
            });
            SetCommisionValue();
            RefreshRow(ref RowId);
            return true;
        }

        /// <summary>
        /// ویرایش ردیف
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns></returns>
        internal (bool, RemittanceListViewModel) OnUpdate(int rowId)
        {
            if (RemId != null)
            {
                return (false, new RemittanceListViewModel());
            }
            var itm = List.FirstOrDefault(t => t.RowId == rowId);
            if (itm == null)
                return new(false, new RemittanceListViewModel());
            MaterialId = itm.MaterialId;
            RemId = itm.RremId;
            AmountOf = itm.AmountOf;
            MatPrice = itm.Price;
            Description = itm.Description;
            List.Remove(itm);
            RefreshRow(ref rowId);
            SetCommisionValue();
            return new(true, itm);
        }

        /// <summary>
        /// حذف ردیف
        /// </summary>
        /// <param name="rowId"></param>s
        [RelayCommand]
        private void OnRemove(int rowId)
        {
            var itm = List.FirstOrDefault(t => t.RowId == rowId);
            if (itm == null)
                return;
            if (itm.RremId != Guid.Empty)
            {
                itm.IsDeleted = !itm.IsDeleted;
            }
            else
            {
                List.Remove(itm);
                RefreshRow(ref rowId);
            }
            SetCommisionValue();
        }

        /// <summary>
        /// ثبت فاکتور
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnSumbit()
        {
            #region validation

            if (RemId != null)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ابتدا فیلدهای ویرایشی را ثبت سپس اقدام به ثبت فاکتور نمایید!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (SubmitDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (List == null || !List.Any(t => !t.IsDeleted))
            {
                _snackbarService.Show("خطا", "وارد کردن حداقل یک ردیف الزامیست !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(InvDescription))
            {
                InvDescription = "فاکتور فروش";
            }
            #endregion

            #region UpdateMaterial
            using UnitOfWork db = new();
            var li = new List<RemittanceListViewModel>(List.Where(t => !t.IsDeleted || t.RremId != Guid.Empty));
            foreach (var item in li)
            {
                if (item.IsService) continue;

                if (item.RremId == Guid.Empty)
                {
                    var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, item.AmountOf, false, item.Price);
                    if (!isSuccess)
                    {
                        _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }
                    continue;
                }
                var oldItm = StaticList.First(t => t.RremId.Equals(item.RremId));

                if (oldItm.AmountOf == item.AmountOf)
                    continue;

                if (item.AmountOf < oldItm.AmountOf)
                {
                    var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, oldItm.AmountOf - item.AmountOf, true, item.Price);
                    if (!isSuccess)
                    {
                        _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }
                    continue;
                }
                else
                {
                    var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, item.AmountOf - oldItm.AmountOf, false, item.Price);
                    if (!isSuccess)
                    {
                        _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }
                    continue;
                }

            }
            #endregion

            #region UpdateSellDoc
            var totalInvoicePrice = li.Where(t => !t.IsDeleted).Sum(t => t.TotalPrice);
            var (e, s) = await db.DocumentManager.UpdateSellDocument(InvoiceId, totalInvoicePrice, Commission, InvDescription, SubmitDate.Value, li);
            if (!s)
            {
                _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            await db.SaveChangesAsync();
            _snackbarService.Show("کاربر گرامی", $"ثبت فاکتور با موفقیت انجام شد", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            #endregion

            #region reDirect
            EditInvoiceDetails.InvoiceId = Guid.Empty;

            Type? pageType = NameToPageTypeConverter.Convert("Bill");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
            #endregion
        }

        /// <summary>
        /// به روز رسانی شماره ردیف ها
        /// </summary>
        /// <param name="rowId"></param>
        private void RefreshRow(ref int rowId)
        {
            int row = 1;
            foreach (var item in List.OrderBy(t => t.RowId))
            {
                item.RowId = row;
                row++;
            }
            rowId = row;
        }

        /// <summary>
        /// به روز رسانی مبلغ پورسانت
        /// </summary>
        private void SetCommisionValue()
        {
            long total = List.Where(t => !t.IsDeleted).Sum(t => t.TotalPrice);
            TotalPrice = total.ToString("N0");
            if (Commission != null && Commission != 0)
            {
                var com = (long)(total * (Commission / 100));
                Totalcommission = com.ToString("N0");
                total -= com;
            }
            else
            {
                Totalcommission = "0";
            }
            RemainPrice = total.ToString("N0");
        }
        #endregion
    }
}