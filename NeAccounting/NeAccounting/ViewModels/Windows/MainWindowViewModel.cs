using Infrastructure.UnitOfWork;
using NeAccounting.Pages;
using NeAccounting.Views.Pages;
using NeAccounting.Views.Pages.Test;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Novin Acconting";

        [ObservableProperty]
        private string _logInError = "";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home32 },
                TargetPageType = typeof(DashboardPage)
            },

            new NavigationViewItemSeparator(),

            new NavigationViewItem()
            {
                Content = "test",
                Icon = new SymbolIcon { Symbol = SymbolRegular.TextEditStyle20 },
                TargetPageType = typeof(TestPage)
            },
            new NavigationViewItem()
            {
                Content = "تعاریف اولیه",
                Icon = new SymbolIcon { Symbol = SymbolRegular.CollectionsAdd20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {
                #region Customer
                    new NavigationViewItem {Content = "مشتری ها",TargetPageType = typeof(CustomerListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.PeopleTeam16} },
                    new NavigationViewItem(){Content = "افزودن مشتری",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateMaterailPage),Visibility = Visibility.Collapsed,},
                    new NavigationViewItem(){Content = "به روز رسانی مشتری",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateMaterailPage),Visibility = Visibility.Collapsed,},
                #endregion

                #region materials
                    new NavigationViewItem {Content = "اجناس",TargetPageType = typeof(MaterailListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.BuildingRetailMore20} },
                    new NavigationViewItem(){Content = "افزودن اجناس",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateMaterailPage),Visibility = Visibility.Collapsed,},
                    new NavigationViewItem(){Content = "به روز رسانی اجناس",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(UpdateMaterailPage),Visibility = Visibility.Collapsed,},
                #endregion
                
                #region Units 
                    new NavigationViewItem {Content = "واحدها",TargetPageType = typeof(UnitsListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.AppsAddIn16} },

                #endregion

                }
            },
            new NavigationViewItem()
            {
                Content = "عملیات روزانه",
                Icon = new SymbolIcon { Symbol = SymbolRegular. TaskListAdd20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {
                #region Remittance
                    new NavigationViewItem {Content = "پیش فاکتور",TargetPageType = typeof(PayPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Content = "فاکتور فروش",TargetPageType = typeof(CreateSellInvoicePage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem { Content = "فاکتور خرید", TargetPageType = typeof(CreateBuyInvoicePage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem { Content = "فاکتور واسطه‌ای", TargetPageType = typeof(CreateIntermediaryInvoicePage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem { Content = "اجناس برگشتی", TargetPageType = typeof(WorkersListPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Content = "سفارشات",TargetPageType = typeof(PayPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
	            #endregion

                #region Doc
                    new NavigationViewItem {Content = "دریافتی از مشتری",TargetPageType = typeof(RecPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem {Content = "پرداختی به مشتری",TargetPageType = typeof(PayPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
	            #endregion
                    new NavigationViewItem { Content = "هزینه ها", TargetPageType = typeof(CreateCostsPage),Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},

                }
            },
            new NavigationViewItem()
            {
                Content = "سال مالی",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Timeline20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {
                                       new NavigationViewItem { Content = "اتمام سال مالی کنونی", TargetPageType = typeof(WorkersListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.AlignEndHorizontal20} },
                    new NavigationViewItem { Content = "... بازگشت به سال", TargetPageType = typeof(WorkersListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.Rename20} },
                }
            },
            new NavigationViewItem()
            {
                Content = "گزارشات",
                Icon = new SymbolIcon { Symbol = SymbolRegular.ClipboardTaskListLtr20 },

                MenuItems = new ObservableCollection<object>
                {
                    new NavigationViewItem { Content = "صورتحساب", TargetPageType = typeof(BillPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
                    new NavigationViewItem { Content = "صورتحساب جزییات", TargetPageType = typeof(Invoicedetails) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20 } },
                    new NavigationViewItem { Content = "بدهکاران", TargetPageType = typeof(DebtorsListPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem { Content = "طلبکاران", TargetPageType = typeof(CreditorsListPage) ,Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem { Content = "دفتر روزانه", TargetPageType = typeof(DailyBook) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20}},
                    new NavigationViewItem { Content = "دفتر چک", TargetPageType = typeof(Chequebook) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20 }},
                    new NavigationViewItem { Content = "هزینه ها", TargetPageType = typeof(CreateCostsPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20 }},
                    new NavigationViewItem { Content = "بیلان سالانه", TargetPageType = typeof(DebtorsListPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20 }},
                    new NavigationViewItem { Content = "بیلان مشخص", TargetPageType = typeof(DebtorsListPage) , Icon = new SymbolIcon { Symbol = SymbolRegular.CaretRight20 }}
                }
            },

            new NavigationViewItem()
            {
                Content = "پرسنل",
                Icon = new SymbolIcon { Symbol = SymbolRegular.LayerDiagonalPerson20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {
                    new NavigationViewItem { Content = "کارگران", TargetPageType = typeof(WorkersListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.CaretRight20} },
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
                Icon = new SymbolIcon { Symbol = SymbolRegular.CopySelect20 },
                //TargetPageType = typeof(MaterailListPage)
                MenuItems = new ObservableCollection<object>
                {
                    new NavigationViewItem { Content = "تهیه نسخه پشتیبان جدید", TargetPageType = typeof(WorkersListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.DocumentCopy20} },
                    new NavigationViewItem { Content = "بازگردانی اطلاعات", TargetPageType = typeof(WorkersListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.SquareHintArrowBack20} },
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
                TargetPageType = typeof(SettingsPage)
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
                if (await db.userRepository.LogInUser(userName, password))
                {
                    LogInError = "ورود با موفقیت انجام شد !!!";
                    return true;
                }
            }
            LogInError = "عدم تطابق نام کاربری و گذرواژه !!!";
            return false;
        }
    }
}
