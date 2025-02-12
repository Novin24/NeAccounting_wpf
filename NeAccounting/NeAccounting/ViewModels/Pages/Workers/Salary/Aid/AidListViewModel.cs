using DomainShared.Constants;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using System.Security.Principal;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class AidListViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;



        #region Properties
        [ObservableProperty]
        private Guid? _workerId = null;

        /// <summary>
        /// صفحه ای ک داخلش قرار داریم
        /// </summary>
        [ObservableProperty]
        private int _currentPage = 1;


		/// <summary>
		/// تعداد صفحات موجود
		/// </summary>
		[ObservableProperty]
		private int _pageCount = 1;

		[ObservableProperty]
        private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;

        [ObservableProperty]
        private IEnumerable<AidViewModel> _list;

        /// <summary>
        /// غیرفعال بودن سرچ
        /// </summary>
        [ObservableProperty]
        private bool _loding = true;

        /// <summary>
        /// متن نمایشی سرچ
        /// </summary>
        [ObservableProperty]
        private string _placeholderSearch = "در حال بارگذاری ...";
        #endregion

        public AidListViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService contentDialogService)
        {
            _snackbarService = snackbarService;
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
        }

        public void OnNavigatedFrom()
        {
        }

        public async void OnNavigatedTo()
        {
            await InitializeViewModel();
        }

        private async Task InitializeViewModel()
        {
            using UnitOfWork db = new();
            AuSuBox = await db.WorkerManager.GetWorkers();
            var result = await db.WorkerManager.GetAidList(WorkerId, CurrentPage);
			PageCount = result.PageCount;
			CurrentPage = result.CurrentPage;
			List = result.Items;
            Loding = false;
            PlaceholderSearch = "جستجو ...";
        }

        [RelayCommand]
        private async Task OnSearchWorker()
        {
            using UnitOfWork db = new();
			var result = await db.WorkerManager.GetAidList(WorkerId, CurrentPage);
			PageCount = result.PageCount;
			CurrentPage = result.CurrentPage;
			List = result.Items;
		}
		[RelayCommand]
		private async Task OnPageChenge()
		{
			using UnitOfWork db = new();
			var t = await db.WorkerManager.GetAidList(WorkerId, CurrentPage);
			PageCount = t.PageCount;
			List = t.Items;
		}

		[RelayCommand]
        private void OnAddClick(string parameter)
        {
            if (String.IsNullOrWhiteSpace(parameter))
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
        private async Task OnRemoveAid(AidDetails parameter)
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            var result = await _contentDialogService.ShowSimpleDialogAsync(
            new SimpleContentDialogCreateOptions()
            {
                Title = "آیا از حذف اطمینان دارید!!!",
                Content = Application.Current.Resources["DeleteDialogContent"],
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });

            if (result == ContentDialogResult.Primary)
            {
                using UnitOfWork db = new();
                var (error, isSuccess) = await db.WorkerManager.DeleteAid(parameter.WorkerId, parameter.PersianYear, parameter.PersianMonth, parameter.Id);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                await OnSearchWorker();
            }
        }

        [RelayCommand]
        private async Task OnUpdateAid(AidDetails parameter)
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            Type? pageType = NameToPageTypeConverter.Convert("UpdateFinancialAid");

            if (pageType == null)
            {
                return;
            }
            var service = _navigationService.GetNavigationControl();

            var aid = List.First(t => t.Details.Id == parameter.Id);

            using UnitOfWork db = new();
            var list = await db.WorkerManager.GetAidList(WorkerId, CurrentPage);
            var context = new UpdateFinancialAidPage(new UpdateFinancialAidViewModel(_navigationService, _snackbarService)
            {
                WorkerId = parameter.WorkerId,
                AmountOf = aid.AmountPrice,
                Description = aid.Description,
                PersonnelName = aid.Name,
                SubmitDate = aid.SubmitDate,
                AidId = parameter.Id,
                List = list.Items,
                PersonnelId = aid.PersonelId
            });

            service.Navigate(pageType, context);
        }
    }
}
