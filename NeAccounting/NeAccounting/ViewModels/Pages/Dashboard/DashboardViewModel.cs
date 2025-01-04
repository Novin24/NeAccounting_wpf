using DomainShared.Constants;
using DomainShared.Notifications;
using Infrastructure.UnitOfWork;
using NeAccounting.Models;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using Wpf.Ui.Controls;
using static Stimulsoft.Client.Designer.Images.StiDesignerWpfImages;

namespace NeAccounting.ViewModels
{
	public partial class DashboardViewModel : ObservableObject, INavigationAware
	{
		private bool _isInitialized = false;

		[ObservableProperty]
		private string _userName = "";

		[ObservableProperty]
		private string _logInTime = "";

		[ObservableProperty]
		private ObservableCollection<NotifViewModel> _notifs;

		public async void OnNavigatedTo()
		{
			if (!_isInitialized)
				await InitializeViewModel();
		}

		public void OnNavigatedFrom()
		{
		}

		private async Task InitializeViewModel()
		{
			UserName = " کاربر  :  " + CurrentUser.CurrentFullName;
			LogInTime = " ورود به برنامه : " + CurrentUser.LogInTime;
			List<NotifViewModel> t = [];

			using (BaseUnitOfWork db = new())
			{
				t= await db.NotifRepository.GetNotifs();
			}

			using (UnitOfWork UW = new())
			{
				t.AddRange(await UW.MaterialManager.GetMaterailforDashboard());

			};

			Notifs = new ObservableCollection<NotifViewModel>(t);



			_isInitialized = true;
		}

	}
}
