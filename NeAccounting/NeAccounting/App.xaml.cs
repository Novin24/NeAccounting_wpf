// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Infrastructure.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NeAccounting.Controls;
using NeAccounting.Resources;
using NeAccounting.Services;
using NeAccounting.ViewModels;
using NeAccounting.ViewModels.Pages;
using NeAccounting.Views.Pages;
using NeAccounting.Views.Pages.Test;
using NeAccounting.Windows;
using NeApplication.Services;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Wpf.Ui;

namespace NeAccounting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {
                #region Main
                services.AddHostedService<ApplicationHostService>();
                services.AddSingleton<MainWindow>();

                //services.AddSingleton<LoadingWindow>(); 
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<WindowsProviderService>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<ISnackbarService, SnackbarService>();
                services.AddSingleton<IContentDialogService, ContentDialogService>();
                #endregion

                #region WatingWindow
                services.AddSingleton<WatingWindow>();
                services.AddSingleton<WatingWindowViewModel>();
                #endregion

                #region Dashboard
                services.AddTransient<DashboardPage>();
                services.AddTransient<DashboardViewModel>();
                #endregion

                #region worker
                services.AddTransient<WorkersListPage>();
                services.AddTransient<WorkerListViewModel>();

                services.AddTransient<CreateWorkerPage>();
                services.AddTransient<CreateWorkerViewModel>();

                services.AddTransient<UpdateWorkerPage>();
                services.AddTransient<UpdateWorkerViewModel>();

                #region Salary
                services.AddTransient<SalaryListPage>();
                services.AddTransient<SalaryListViewModel>();

                services.AddTransient<CreateSalaryPage>();
                services.AddTransient<CreateSalaryViewModel>();

                services.AddTransient<UpdateSalaryPage>();
                services.AddTransient<UpdateSalaryViewModel>();
                #endregion

                #region aid
                services.AddTransient<FinancialAidListPage>();
                services.AddTransient<AidListViewModel>();

                services.AddTransient<CreateFinancialAidPage>();
                services.AddTransient<CreateFinancialAidViewModel>();

                services.AddTransient<UpdateFinancialAidPage>();
                services.AddTransient<UpdateFinancialAidViewModel>();
                #endregion

                #region function
                services.AddTransient<FunctionListPage>();
                services.AddTransient<FunctionListViewModel>();

                services.AddTransient<CreateFunctionPage>();
                services.AddTransient<CreateFunctionViewModel>();

                services.AddTransient<UpdateFunctionPage>();
                services.AddTransient<UpdateFunctionViewModel>();
                #endregion

                #endregion

                #region Customer
                services.AddTransient<CustomerListPage>();
                services.AddTransient<CustomerListViewModel>();

                services.AddTransient<CreateCustomerWindow>();
                services.AddTransient<HotCreateCustomerViewModel>();

                services.AddTransient<CreateCustomerPage>();
                services.AddTransient<CreateCustomerViewModel>();

                services.AddTransient<UpdateCustomerPage>();
                services.AddTransient<UpdateCustomerViewModel>();
                #endregion

                services.AddTransient<TestPage>();
                services.AddTransient<TestViewModel>();

                #region Cheque
                services.AddTransient<ChequebookPage>();
                services.AddTransient<ChequebookViewModel>();

                services.AddTransient<CheckDetailsPage>();
                services.AddTransient<DetailsChequeViewModel>();

                services.AddTransient<CreatePayChequePage>();
                services.AddTransient<CreatePayChequeViewModel>();

                services.AddTransient<CreateRecChequePage>();
                services.AddTransient<CreateRecChequeViewModel>();

                services.AddTransient<CreateGuarantChequePage>();
                services.AddTransient<CreateGuarantChequeViewModel>();

                services.AddTransient<UpdateChequePage>();
                services.AddTransient<UpdateChequeViewModel>();

                services.AddTransient<TransferChequePage>();
                services.AddTransient<TransferChequeViewModel>();

                services.AddTransient<UpdateTransferChequePage>();
                services.AddTransient<UpdateTransferChequeViewModel>();
                #endregion

                #region Recived
                services.AddTransient<CreateRecPage>();
                services.AddTransient<CreateRecViewModel>();

                services.AddTransient<UpdateRecDocPage>();
                services.AddTransient<UpdateRecDocViewModel>();
                #endregion

                #region Invoice

                #region Previewinvoice
                services.AddTransient<PreviewinvoicePage>();
                services.AddTransient<PreviewinvoiceViewModel>();
                #endregion

                #region BuyInvoice
                services.AddTransient<CreateBuyInvoicePage>();
                services.AddTransient<CreateBuyInvoiceViewModel>();
                services.AddTransient<UpdateBuyInvoicePage>();
                services.AddTransient<UpdateBuyInvoiceViewModel>();
                #endregion

                #region IntermediaryInvoice
                services.AddTransient<CreateIntermediaryInvoicePage>();
                //services.AddTransient<CreateIntermediaryInvoiceViewModel>();
                services.AddTransient<UpdateIntermediaryInvoicePage>();
                //services.AddTransient<UpdateIntermediaryInvoiceViewModel>();
                #endregion

                #region SellInvoice
                services.AddTransient<CreateSellInvoicePage>();
                services.AddTransient<CreateSellInvoiceViewModel>();

                services.AddTransient<UpdateSellInvoicePage>();
                services.AddTransient<UpdateSellInvoiceViewModel>();
                #endregion

                #region ReturnGoods
                services.AddTransient<FromTheSellPage>();
                services.AddTransient<FromTheSellViewModel>();

                services.AddTransient<FromTheBuyPage>();
                services.AddTransient<FromTheBuyViewModel>();

                services.AddTransient<UpdateFromSellPage>();
                services.AddTransient<UpdateFromTheSellViewModel>();

                services.AddTransient<UpdateFromBuyPage>();
                services.AddTransient<UpdateFromTheBuyViewModel>();
                #endregion

                #endregion

                #region Expense
                services.AddTransient<CreateExpencePage>();
                services.AddTransient<CreateExpenceViewModel>();

                services.AddTransient<UpdateExpencePage>();
                services.AddTransient<UpdateExpenceViewModel>();

                services.AddTransient<ExpencesListPage>();
                services.AddTransient<ExpencelistViewModel>();
                #endregion

                #region Payment
                services.AddTransient<CreatePayDocPage>();
                services.AddTransient<CreatePayDocViewModel>();

                services.AddTransient<UpdatePayDocPage>();
                services.AddTransient<UpdatePayDocViewModel>();
                #endregion

                #region Units 

                services.AddTransient<UnitsListPage>();
                services.AddTransient<UnitViewModel>();

                #endregion

                #region ProfitOrLess
                services.AddTransient<ProfitOrLessPage>();
                services.AddTransient<ProfitOrLessViewModel>();
                #endregion

                #region FiscalYear 

                services.AddTransient<FiscalYearListPage>();
                services.AddTransient<FiscalYearViewModel>();
                services.AddTransient<CreateFiscalYear>();
                services.AddTransient<CreateFinancialViewModel>();

                #endregion

                #region materials
                services.AddTransient<CreateMaterialWindow>();
                services.AddTransient<HotCreateMaterailViewModel>();

                services.AddTransient<CreateMaterailPage>();
                services.AddTransient<CreateMaterailViewModel>();

                services.AddTransient<MaterailListPage>();
                services.AddTransient<MaterailListViewModel>();

                services.AddTransient<UpdateMaterailPage>();
                services.AddTransient<UpdateMaterailViewModel>();
                #endregion

                #region Services
                services.AddTransient<CreateServicePage>();
                services.AddTransient<CreateServiceViewModel>();

                services.AddTransient<UpdateServicePage>();
                services.AddTransient<UpdateServiceViewModel>();


                #endregion

                #region Reports
                services.AddTransient<BillPage>();
                services.AddTransient<BillListViewModel>();

                services.AddTransient<InvoicedetailsPage>();
                services.AddTransient<InvoicedetailsViewModel>();

                services.AddTransient<MaterialReportPage>();
                services.AddTransient<MaterialReportViewModel>();

                services.AddTransient<DebtorsListPage>();
                services.AddTransient<DebtorsViewModel>();

                services.AddTransient<CreditorsListPage>();
                services.AddTransient<CreditorsViewModel>();

                services.AddTransient<DailyBookPage>();
                services.AddTransient<DalyBookViewModel>();
                #endregion

                #region UserControl
                services.AddTransient<Pagination>();
                services.AddTransient<PersianDatePicker>();
                #endregion

                #region Setting
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();
                #endregion

                #region DbContext
                services.AddDbContext<NovinDbContext>();
                services.AddDbContext<BaseDomainDbContext>();
                #endregion

                #region Services
                services.AddTransient<IPrintServices, PrintServices>();
				#endregion

				#region Backup
				services.AddTransient<BackupPage>();
				services.AddTransient<BackupViewModel>();

				services.AddTransient<ExporteCustomersPage>();
				services.AddTransient<ExporteCustomerViewModel>();

				services.AddTransient<ImportCustomersPage>();
				services.AddTransient<ImportCustomerViewModel>();
				#endregion

				#region ChangePassword
				services.AddTransient<ChangePassword>();
                services.AddTransient<ChangePassViewModel>();
                #endregion

                #region Notification
                services.AddTransient<NotificationListPage>();
                services.AddTransient<NotifListViewModel>();

                services.AddTransient<CreateNotificationPage>();
                services.AddTransient<CreateNotifViewModel>();

                services.AddTransient<UpdateNotificationPage>();
                services.AddTransient<UpdateNotifViewModel>();
                #endregion

            }).Build();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            _ = new Mutex(true, "NovinAcoounting", out bool runed);
            if (!runed)
            {
                Wpf.Ui.Controls.MessageBox ms = new Wpf.Ui.Controls.MessageBox()
                {
                    Title = "کاربر گرامی",
                    FontFamily = new System.Windows.Media.FontFamily("Calibri"),
                    Content = "برنامه حسابداری در حال اجراست !!!"
                };
                await ms.ShowDialogAsync();
            }
            _host.Start();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
