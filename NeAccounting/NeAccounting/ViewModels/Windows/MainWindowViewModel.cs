using DomainShared.Constants;
using DomainShared.Enums.Themes;
using Infrastructure.BaseRepositories;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using NeAccounting.Views.Pages.Test;
using System.Collections.ObjectModel;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private string _applicationTitle = "Novin Acconting";

        [ObservableProperty]
        private string _logInError = "";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "داشبورد",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home32 },
                TargetPageType = typeof(DashboardPage)
            },

            new NavigationViewItemSeparator(),

            //new NavigationViewItem()
            //{
            //    Content = "test",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.TextEditStyle20 },
            //    TargetPageType = typeof(TestPage)
            //},
            new NavigationViewItem()
            {
                Content = "تعاریف اولیه",
                Icon = new SymbolIcon { Symbol = SymbolRegular.CollectionsAdd20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {
                #region Customer
                    new NavigationViewItem {Content = "مشتری ها",TargetPageType = typeof(CustomerListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.PeopleTeam16} },
                    new NavigationViewItem(){Content = "افزودن مشتری",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateCustomerPage),Visibility = Visibility.Collapsed,},
                    new NavigationViewItem(){Content = "به روز رسانی مشتری",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateCustomerPage),Visibility = Visibility.Collapsed,},
                #endregion

                #region materials
                    new NavigationViewItem {Content = "اجناس و خدمات",TargetPageType = typeof(MaterailListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.BuildingRetailMore20} },
                    new NavigationViewItem(){Content = "افزودن اجناس",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateMaterailPage),Visibility = Visibility.Collapsed,},
                    new NavigationViewItem(){Content = "به روز رسانی اجناس",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateMaterailPage),Visibility = Visibility.Collapsed,},
                #endregion
                    
                #region Services
                    new NavigationViewItem(){Content = "افزودن خدمات",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateServicePage),Visibility = Visibility.Collapsed,},
                    new NavigationViewItem(){Content = "به روز رسانی خدمات",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateServicePage),Visibility = Visibility.Collapsed,},
                #endregion
                
                #region Units 
                    new NavigationViewItem {Content = "واحدها",TargetPageType = typeof(UnitsListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.AppsAddIn16} },

                #endregion

                }
            },
            new NavigationViewItem()
            {
                Content = " عملیات روزانه",
                Icon = new SymbolIcon { Symbol = SymbolRegular. TaskListAdd20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {
                #region Remittance
                    new NavigationViewItem {Content = "پیش فاکتور",TargetPageType = typeof(PreviewinvoicePage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Content = "فاکتور فروش",TargetPageType = typeof(CreateSellInvoicePage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem { Content = "فاکتور خرید", TargetPageType = typeof(CreateBuyInvoicePage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    //new NavigationViewItem { Content = "فاکتور واسطه‌ای", TargetPageType = typeof(CreateIntermediaryInvoicePage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    //new NavigationViewItem { Content = "اجناس برگشتی", TargetPageType = typeof(WorkersListPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    //new NavigationViewItem {Content = "سفارشات",TargetPageType = typeof(CreatePayDocPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
	            #endregion

                #region Doc
                    new NavigationViewItem {Content = "دریافتی از مشتری",TargetPageType = typeof(CreateRecPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Content = "پرداختی به مشتری",TargetPageType = typeof(CreatePayDocPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Content = "ثبت چک دریافتی ",TargetPageType = typeof(CreateRecChequePage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Content = "ثبت چک پرداختی ",TargetPageType = typeof(CreatePayChequePage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
	            #endregion
                    new NavigationViewItem { Content = "هزینه ها", TargetPageType = typeof(CreateExpencePage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},

                }
            },
            new NavigationViewItem()
            {
                Content = "سال مالی",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Timeline20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {
                    new NavigationViewItem { Content = "اتمام سال مالی کنونی", TargetPageType = typeof(CreateFiscalYear) , Icon = new SymbolIcon{ Symbol = SymbolRegular.AlignEndHorizontal20} },
                    new NavigationViewItem { Content = "... بازگشت به سال", TargetPageType = typeof(FiscalYearListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.Rename20} },
                }
            },
            new NavigationViewItem()
            {
                Content = "گزارشات",
                Icon = new SymbolIcon { Symbol = SymbolRegular.ClipboardTaskListLtr20 },

                MenuItems = new ObservableCollection<object>
                {
                    new NavigationViewItem { Content = "صورتحساب", TargetPageType = typeof(BillPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem {Content = "به روز رسانی پرداختی به مشتری",TargetPageType = typeof(UpdatePayDocPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20},Visibility = Visibility.Collapsed},
                    new NavigationViewItem {Content = "به روز رسانی دریافتی به مشتری",TargetPageType = typeof(UpdateRecDocPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20},Visibility = Visibility.Collapsed},
                    new NavigationViewItem {Content = "به روز رسانی فاکتور فروش",TargetPageType = typeof(UpdateSellInvoicePage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20},Visibility =Visibility.Collapsed },
                    new NavigationViewItem { Content = "صورتحساب جزییات", TargetPageType = typeof(InvoicedetailsPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20 } },
                    new NavigationViewItem { Content = "صورت سود و زیان", TargetPageType = typeof(ProfitOrLessPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20 }},
                    new NavigationViewItem { Content = "گزارش اجناس", TargetPageType = typeof(MaterialReportPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem { Content = "بدهکاران", TargetPageType = typeof(DebtorsListPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem { Content = "طلبکاران", TargetPageType = typeof(CreditorsListPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem { Content = "دفتر روزانه", TargetPageType = typeof(DailyBookPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem { Content = "دفتر چک", TargetPageType = typeof(ChequebookPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20 }},
                    new NavigationViewItem(){Content = "ایجاد چک پرداختی",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreatePayChequePage),Visibility = Visibility.Collapsed,},
                    new NavigationViewItem(){Content = "ایجاد چک دریافتی",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateRecChequePage),Visibility = Visibility.Collapsed,},
                    new NavigationViewItem(){Content = "ایجاد چک ضمانتی",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateGuarantChequePage),Visibility = Visibility.Collapsed,},
                    new NavigationViewItem { Content = "هزینه ها", TargetPageType = typeof(ExpencesListPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20 }},
                }
            },

            new NavigationViewItem()
            {
                Content = "پرسنل",
                Icon = new SymbolIcon { Symbol = SymbolRegular.LayerDiagonalPerson20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {
                    new NavigationViewItem { Content = "کارگران و کارمندان", TargetPageType = typeof(WorkersListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem(){Content = "به روز رسانی پرسنل",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateWorkerPage),
                        Visibility = Visibility.Collapsed},
                    new NavigationViewItem(){Content = "افزودن پرسنل",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateWorkerPage),
                        Visibility = Visibility.Collapsed},


                    new NavigationViewItem { Content = "فیش حقوقی", TargetPageType = typeof(SalaryListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem(){Content = "صدور فیش حقوقی",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateSalaryPage),
                        Visibility = Visibility.Collapsed},
                    new NavigationViewItem(){Content = "به روز رسانی فیش حقوقی",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateSalaryPage),
                        Visibility = Visibility.Collapsed},


                    new NavigationViewItem(){Content = "مساعده",TargetPageType = typeof(FinancialAidListPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem(){Content = "افزودن مساعده",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateFinancialAidPage),
                        Visibility = Visibility.Collapsed},
                    new NavigationViewItem(){Content = "ویرایش مساعده",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateFinancialAidPage),
                        Visibility = Visibility.Collapsed},


                    new NavigationViewItem(){Content = "کارکرد",TargetPageType = typeof(FunctionListPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem(){Content = "افزودن کارکرد",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateFunctionPage),
                        Visibility = Visibility.Collapsed},
                    new NavigationViewItem(){Content = "ویرایش کارکر",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateFunctionPage),
                        Visibility = Visibility.Collapsed},

                }
            },
            new NavigationViewItem()
            {
                Content = "پشتبان گیری",
                Icon = new SymbolIcon { Symbol = SymbolRegular. TaskListAdd20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {
                
                    new NavigationViewItem(){Content = "پشتبان گیری از پایگاه داده",TargetPageType = typeof(BackupPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CopySelect20}},
					new NavigationViewItem {Content = "وارد کردن مشتریان",TargetPageType = typeof(ImportCustomersPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CopySelect20}},
					new NavigationViewItem {Content = "خروجی گرفتن از مشتریان",TargetPageType = typeof(ExporteCustomersPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CopySelect20}},
					new NavigationViewItem {Content = "وارد کردن اجناس",TargetPageType = typeof(ImportMaterailsPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CopySelect20}},
					new NavigationViewItem {Content = "خروجی گرفتن از اجناس",TargetPageType = typeof(ExporteMaterailsPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CopySelect20}},

				}
            },
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "تنظیمات کاربری",
                Icon = new SymbolIcon { Symbol = SymbolRegular.PersonSettings20 },
                MenuItems = new ObservableCollection<object>
                {
                #region ChangePassword
                    new NavigationViewItem {Content = "تغییر رمز عبور",TargetPageType = typeof(ChangePassword) , Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                #endregion
                    
                #region ChangePassword
                    new NavigationViewItem {Content = "یادآورها",TargetPageType = typeof(NotificationListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem(){Content = "ایجاد یادآور",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateNotificationPage),
                        Visibility = Visibility.Collapsed},
                #endregion

                }
            },
            new NavigationViewItem()
            {
                Content = "تنظیمات برنامه",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings20 },
                TargetPageType = typeof(SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
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
            using (BaseUnitOfWork db = new())
            {
                if (await db.UserRepository.LogInUser(userName, password))
                {
                    var r = await db.FinancialYearRepository.GetActiveYear();
                    if (!r.isSucces)
                    {
                        LogInError = "!!!";
                        return true;
                    }
                    NeAccountingConstants.NvoinDbConnectionStrint = r.databaseName;
                    NeAccountingConstants.NvoinCurentDb= r.databaseTitle;
                    NeAccountingConstants.ReadOnlyMode = !r.isCurrent;

					// بارگذاری تم کاربر پس از ورود
					var theme = await db.UserRepository.LoadUserTheme(CurrentUser.CurrentUserId);
                    if(theme == Theme.Dark)
						ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                    else
                        ApplicationThemeManager.Apply(ApplicationTheme.Light);
					LogInError = "ورود با موفقیت انجام شد !!!";
                    return true;
                }
            }
            LogInError = "عدم تطابق نام کاربری و گذرواژه !!!";
            return false;
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
    }
}
