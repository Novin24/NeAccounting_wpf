using System.Collections.ObjectModel;
using DomainShared.Constants;
using DomainShared.Enums.Themes;
using DomainShared.ViewModels.Menu;
using Infrastructure.UnitOfWork;
using NeAccounting.Views.Pages;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

public partial class LoginWindowViewModel : ObservableObject
{
    public LoginWindowViewModel()
    {
    }

    [ObservableProperty]
    private string _applicationTitle = "Novin Acconting";

    [ObservableProperty]
    private string _logInError = "";

    private ObservableCollection<NavigationViewItem> MainItems = new()
        {
            new NavigationViewItem()
            {
                Tag="InitialDefinitions",
                Content = "تعاریف اولیه",
                Icon = new SymbolIcon { Symbol = SymbolRegular.CollectionsAdd20 },
                MenuItems = new ObservableCollection<NavigationViewItem>
                {
                #region Customer
                    new() {Tag="Customers",Content = "مشتری ها",TargetPageType = typeof(CustomerListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.PeopleTeam16} },
                    new() {Content = "افزودن مشتری",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateCustomerPage),Visibility = Visibility.Collapsed,},
                    new() {Content = "به روز رسانی مشتری",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateCustomerPage),Visibility = Visibility.Collapsed,},
                #endregion

                #region materials
                    new() {Tag="GoodsServices", Content = "اجناس و خدمات",TargetPageType = typeof(MaterailListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.BuildingRetailMore20} },
                    new() {Content = "افزودن اجناس",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateMaterailPage),Visibility = Visibility.Collapsed,},
                    new() {Content = "به روز رسانی اجناس",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateMaterailPage),Visibility = Visibility.Collapsed,},
                #endregion
                    
                #region Services
                    new() {Content = "افزودن خدمات",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateServicePage),Visibility = Visibility.Collapsed,},
                    new() {Content = "به روز رسانی خدمات",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateServicePage),Visibility = Visibility.Collapsed,},
                #endregion
                
                #region Units 
                    new() {Tag="Units",Content = "واحدها",TargetPageType = typeof(UnitsListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.AppsAddIn16} },

                #endregion
                }
            },
            new NavigationViewItem()
            {
                Tag="DailyOperations",
                Content = " عملیات روزانه",
                Icon = new SymbolIcon { Symbol = SymbolRegular. TaskListAdd20 },
                MenuItems = new ObservableCollection<NavigationViewItem>
                {
                #region Remittance
                    new() {Tag="PreInvoice", Content = "پیش فاکتور",TargetPageType = typeof(PreviewinvoicePage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new() {Tag="SalesInvoice",Content = "فاکتور فروش",TargetPageType = typeof(CreateSellInvoicePage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new() {Tag="PurchaseInvoice", Content = "فاکتور خرید", TargetPageType = typeof(CreateBuyInvoicePage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    //new NavigationViewItem { Content = "فاکتور واسطه‌ای", TargetPageType = typeof(CreateIntermediaryInvoicePage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    //new NavigationViewItem { Content = "اجناس برگشتی", TargetPageType = typeof(WorkersListPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    //new NavigationViewItem {Content = "سفارشات",TargetPageType = typeof(CreatePayDocPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
	            #endregion

                #region Doc
                    new() {Tag="ReceiveFromCustomer",Content = "دریافتی از مشتری",TargetPageType = typeof(CreateRecPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new() {Tag="PayToCustomer",Content = "پرداختی به مشتری",TargetPageType = typeof(CreatePayDocPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new() {Tag="ReceiveCheque",Content = "ثبت چک دریافتی ",TargetPageType = typeof(CreateRecChequePage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new() {Tag="PayCheque",Content = "ثبت چک پرداختی ",TargetPageType = typeof(CreatePayChequePage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
	            #endregion
                    new NavigationViewItem {Tag="Expenses", Content = "هزینه ها", TargetPageType = typeof(CreateExpencePage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},

                }
            },
            new NavigationViewItem()
            {
                Tag="DailyOperations",
                Content = "سال مالی",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Timeline20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {
                    new NavigationViewItem { Tag="DailyOperations",Content = "اتمام سال مالی کنونی", TargetPageType = typeof(CreateFiscalYear) , Icon = new SymbolIcon{ Symbol = SymbolRegular.AlignEndHorizontal20} },
                    new NavigationViewItem { Tag    ="DailyOperations",Content = "... بازگشت به سال", TargetPageType = typeof(FiscalYearListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.Rename20} },
                }
            },
            new NavigationViewItem()
            {
                Tag="Reports",
                Content = "گزارشات",
                Icon = new SymbolIcon { Symbol = SymbolRegular.ClipboardTaskListLtr20 },

                MenuItems = new ObservableCollection<object>
                {
                    new NavigationViewItem {Tag = "BalanceSheet", Content = "صورتحساب", TargetPageType = typeof(BillPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem {Content = "به روز رسانی پرداختی به مشتری",TargetPageType = typeof(UpdatePayDocPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20},Visibility = Visibility.Collapsed},
                    new NavigationViewItem {Content = "به روز رسانی دریافتی به مشتری",TargetPageType = typeof(UpdateRecDocPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20},Visibility = Visibility.Collapsed},
                    new NavigationViewItem {Content = "به روز رسانی فاکتور فروش",TargetPageType = typeof(UpdateSellInvoicePage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20},Visibility =Visibility.Collapsed },
                    new NavigationViewItem {Tag = "BalanceDetails", Content = "صورتحساب جزییات", TargetPageType = typeof(InvoicedetailsPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20 } },
                    new NavigationViewItem {Tag = "ProfitLoss", Content = "صورت سود و زیان", TargetPageType = typeof(ProfitOrLessPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20 }},
                    new NavigationViewItem {Tag = "GoodsReport", Content = "گزارش اجناس", TargetPageType = typeof(MaterialReportPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem {Tag = "Debtors", Content = "بدهکاران", TargetPageType = typeof(DebtorsListPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Tag = "Creditors", Content = "طلبکاران", TargetPageType = typeof(CreditorsListPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Tag = "DailyLedger", Content = "دفتر روزانه", TargetPageType = typeof(DailyBookPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Tag = "ChequeLedger", Content = "دفتر چک", TargetPageType = typeof(ChequebookPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20 }},
                    new NavigationViewItem {Content = "ایجاد چک پرداختی",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreatePayChequePage),Visibility = Visibility.Collapsed,},
                    new NavigationViewItem {Content = "ایجاد چک دریافتی",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateRecChequePage),Visibility = Visibility.Collapsed,},
                    new NavigationViewItem {Content = "ایجاد چک ضمانتی",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateGuarantChequePage),Visibility = Visibility.Collapsed,},
                    new NavigationViewItem {Tag = "ExpenseLedger", Content = "هزینه ها", TargetPageType = typeof(ExpencesListPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20 }},
                }
            },
            new NavigationViewItem()
            {
                Tag="Personnel",
                Content = "پرسنل",
                Icon = new SymbolIcon { Symbol = SymbolRegular.LayerDiagonalPerson20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new List<object>
                {
                    new NavigationViewItem {Tag = "Employees", Content = "کارگران و کارمندان", TargetPageType = typeof(WorkersListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem {Content = "به روز رسانی پرسنل",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateWorkerPage),Visibility = Visibility.Collapsed},
                    new NavigationViewItem {Content = "افزودن پرسنل",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateWorkerPage),Visibility = Visibility.Collapsed},


                    new NavigationViewItem {Tag = "SalarySlip", Content = "فیش حقوقی", TargetPageType = typeof(SalaryListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem {Content = "صدور فیش حقوقی",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateSalaryPage),Visibility = Visibility.Collapsed},
                    new NavigationViewItem {Content = "به روز رسانی فیش حقوقی",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateSalaryPage),Visibility = Visibility.Collapsed},


                    new NavigationViewItem {Tag = "Assistants", Content = "مساعده",TargetPageType = typeof(FinancialAidListPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Content = "افزودن مساعده",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateFinancialAidPage),Visibility = Visibility.Collapsed},
                    new NavigationViewItem {Content = "ویرایش مساعده",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateFinancialAidPage),Visibility = Visibility.Collapsed},


                    new NavigationViewItem {Tag = "Attendance", Content = "کارکرد",TargetPageType = typeof(FunctionListPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Content = "افزودن کارکرد",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateFunctionPage),Visibility = Visibility.Collapsed},
                    new NavigationViewItem {Content = "ویرایش کارکر",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateFunctionPage),Visibility = Visibility.Collapsed},

                }
            },
            new NavigationViewItem()
            {
                Tag = "Backup",
                Content = "پشتبان گیری",
                Icon = new SymbolIcon { Symbol = SymbolRegular. TaskListAdd20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {

                    new NavigationViewItem {Tag = "BackupDatabase", Content = "پشتبان گیری از پایگاه داده",TargetPageType = typeof(BackupPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CopySelect20}},
                    new NavigationViewItem {Tag = "ImportCustomers", Content = "وارد کردن مشتریان",TargetPageType = typeof(ImportCustomersPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CopySelect20}},
                    new NavigationViewItem {Tag = "ExportCustomers", Content = "خروجی گرفتن از مشتریان",TargetPageType = typeof(ExporteCustomersPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CopySelect20}},
                    new NavigationViewItem {Tag = "ImportGoods", Content = "وارد کردن اجناس",TargetPageType = typeof(ImportMaterailsPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CopySelect20}},
                    new NavigationViewItem {Tag = "ExportGoods", Content = "خروجی گرفتن از اجناس",TargetPageType = typeof(ExporteMaterailsPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CopySelect20}},

                }
            },
        };

    public async Task<bool> LogIn(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName))
        {
            LogInError = "وارد کردن نام کاربری الزامیست !!!";
            return false;
        }

        if (string.IsNullOrEmpty(password))
        {
            LogInError = "وارد کردن گذرواژه الزامیست !!!";
            return false;
        }
        using BaseUnitOfWork db = new();
        var (isSuccess, error) = await db.UserRepository.LogInUser(userName, password);

        if (isSuccess)
        {
            SetUserMenu(await db.UserRepository.GetUserMenu());
            var r = await db.FinancialYearRepository.GetActiveYear();
            if (!r.isSucces)
            {
                LogInError = "!!!";
                return true;
            }
            NeAccountingConstants.NvoinDbConnectionStrint = r.databaseName;
            NeAccountingConstants.NvoinCurentDb = r.databaseTitle;
            NeAccountingConstants.ReadOnlyMode = !r.isCurrent;

            // بارگذاری تم کاربر پس از ورود
            var theme = await db.UserRepository.LoadUserTheme(CurrentUser.CurrentUserId);
            if (theme == Theme.Dark)
                ApplicationThemeManager.Apply(ApplicationTheme.Dark);
            else
                ApplicationThemeManager.Apply(ApplicationTheme.Light);
            LogInError = "ورود با موفقیت انجام شد !!!";
            return true;
        }

        LogInError = error;
        return false;
    }

    private ObservableCollection<object> SetUserMenu(List<UserMenuDto> menus)
    {

        var tempMenuList = new List<object>();
        foreach (var parentItem in menus)
        {
            var parentmenu = MainItems.FirstOrDefault(t => t.Tag.ToString() == parentItem.Name);
            if (parentmenu == null) continue;

            var availableSubMenus = parentmenu.MenuItems.Cast<NavigationViewItem>().ToList();
            parentmenu.MenuItems.Clear();
            foreach (var subitem in parentItem.Children)
            {
                var submenu = availableSubMenus.FirstOrDefault(t => t.Name == subitem.Name);
                if (submenu != null) parentmenu.MenuItems.Add(submenu);
            }
            tempMenuList.Add(parentmenu);
        }
       return new ObservableCollection<object>(tempMenuList);

    }
}
