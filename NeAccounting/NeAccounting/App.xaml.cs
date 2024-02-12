// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Infrastructure.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NeAccounting.Pages;
using NeAccounting.Services;
using NeAccounting.ViewModels;
using NeAccounting.ViewModels.Pages;
using NeAccounting.Views.Pages;
using NeAccounting.Views.Pages.Test;
using NeAccounting.Windows;
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
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<ISnackbarService, SnackbarService>();
                services.AddSingleton<IContentDialogService, ContentDialogService>();
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

                services.AddTransient<CreateCustomerPage>();
                services.AddTransient<CreateCustomerViewModel>();

                services.AddTransient<UpdateCustomerPage>();
                services.AddTransient<UpdateCustomerViewModel>();
                #endregion

                services.AddTransient<TestPage>();
                services.AddTransient<TestViewModel>();

                #region Recived
                services.AddTransient<RecPage>();
                services.AddTransient<RecViewModel>();
                #endregion

                #region Invoice
                services.AddTransient<CreateSellInvoicePage>();
                //services.AddTransient<CreateSellInvoiceViewModel>();
                services.AddTransient<UpdateSellInvoicePage>();
                //services.AddTransient<UpdateSellInvoiceViewModel>();

                services.AddTransient<CreateBuyInvoicePage>();
                //services.AddTransient<CreateBuyInvoiceViewModel>();
                services.AddTransient<UpdateBuyInvoicePage>();
                //services.AddTransient<UpdateBuyInvoiceViewModel>();

                services.AddTransient<CreateIntermediaryInvoicePage>();
                //services.AddTransient<CreateIntermediaryInvoiceViewModel>();
                services.AddTransient<UpdateIntermediaryInvoicePage>();
                //services.AddTransient<UpdateIntermediaryInvoiceViewModel>();
                #endregion

                #region SellInvoice
                services.AddTransient<CreateSellInvoicePage>();
                services.AddTransient<CreateSellInviceViewModel>();

                #endregion

                #region Expence
                services.AddTransient<CreateExpencePage>();
                //services.AddTransient<CreateExpenceViewModel>();

                services.AddTransient<UpdateExpencePage>();
                //services.AddTransient<UpdateExpenceViewModel>();

                services.AddTransient<ExpencesListPage>();
                //services.AddTransient<ExpencesListViewModel>();
                #endregion

                #region Payment
                services.AddTransient<PayPage>();
                services.AddTransient<PayViewModel>();
                #endregion

                #region Units 

                services.AddTransient<UnitsListPage>();

                #endregion

                #region materials
                services.AddTransient<CreateMaterailPage>();
                services.AddTransient<CreateMaterailViewModel>();

                services.AddTransient<MaterailListPage>();
                services.AddTransient<MaterailListViewModel>();

                services.AddTransient<UpdateMaterailPage>();
                services.AddTransient<UpdateMaterailViewModel>();
                #endregion

                #region Reports
                services.AddTransient<Bill>();

                services.AddTransient<Invoicedetails>();
                #endregion

                #region Setting
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();
                #endregion

                #region DbContext
                services.AddDbContext<NovinDbContext>();
                services.AddDbContext<BaseDomainDbContext>();
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
        private void OnStartup(object sender, StartupEventArgs e)
        {
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
