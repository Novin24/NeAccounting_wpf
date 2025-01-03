﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using DomainShared.Constants;
using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels.Pages
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = String.Empty;

        [ObservableProperty]
        private string _UserName = String.Empty;

        [ObservableProperty]
        private string _FullName = String.Empty;

        [ObservableProperty]
        private string _FiscalYear = String.Empty;

        [ObservableProperty]
        private Wpf.Ui.Appearance.ApplicationTheme _currentTheme = Wpf.Ui.Appearance.ApplicationTheme.Unknown;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            CurrentTheme = Wpf.Ui.Appearance.ApplicationThemeManager.GetAppTheme();
            AppVersion = $"NeAccounting - {GetAssemblyVersion()}";
            UserName = CurrentUser.CurrentUserName;
            FullName = CurrentUser.CurrentFullName;
            FiscalYear = NeAccountingConstants.NvoinCurentDb;
            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? String.Empty;
        }
		[RelayCommand]
		private async void OnChangeTheme(string parameter)
		{
			using (BaseUnitOfWork db = new BaseUnitOfWork())
			{
				switch (parameter)
				{
					case "theme_light":
						if (CurrentTheme == Wpf.Ui.Appearance.ApplicationTheme.Light)
							break;

						Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Light);
						CurrentTheme = Wpf.Ui.Appearance.ApplicationTheme.Light;

						// به‌روزرسانی تم در دیتابیس
						await db.UserRepository.UpdateUserTheme(CurrentUser.CurrentUserId, DomainShared.Enums.Themes.Theme.Light);
						break;

					default:
						if (CurrentTheme == Wpf.Ui.Appearance.ApplicationTheme.Dark)
							break;

						Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Dark);
						CurrentTheme = Wpf.Ui.Appearance.ApplicationTheme.Dark;

						// به‌روزرسانی تم در دیتابیس
						await db.UserRepository.UpdateUserTheme(CurrentUser.CurrentUserId, DomainShared.Enums.Themes.Theme.Dark);
						break;
				}

				await db.SaveChangesAsync();
			}
		}
	}
}
