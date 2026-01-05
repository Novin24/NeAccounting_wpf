// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NeAccounting.Views.Pages;
using NeAccounting.Views.Pages.Test;
using NeAccounting.Windows;
using Serilog;
using System.Windows.Navigation;

namespace NeAccounting.Services
{
    /// <summary>
    /// Managed host of the application.
    /// </summary>
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ApplicationHostService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await HandleActivationAsync();
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Creates main window during activation.
        /// </summary>
        private async Task HandleActivationAsync()
        {
            await Task.CompletedTask;

            //var loadWindow = _serviceProvider.GetRequiredService<LoadingWindow>();
            //loadWindow.Show();
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Log.Error(e.ExceptionObject as Exception, "An unhandled exception occurred.");
            };
            if (!Application.Current.Windows.OfType<LogInWindow>().Any())
            {
                var loginWindow = _serviceProvider.GetRequiredService<LogInWindow>();
                Application.Current.MainWindow = _serviceProvider.GetRequiredService<LoadingWindow>(); 
                bool? loginResult = loginWindow.ShowDialog();
                if (loginResult == true)
                {
                    //navigationWindow.Loaded += OnNavigationWindowLoaded;
                    var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                    Application.Current.MainWindow = mainWindow;

                    mainWindow.Show();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void OnNavigationWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not MainWindow navigationWindow)
            {
                return;
            }

            navigationWindow.NavigationView.Navigate(typeof(TestPage));
        }
    }
}
