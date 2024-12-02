using DomainShared.Constants;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class FunctionListViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;


        [ObservableProperty]
        private Guid? _workerId = null;

        [ObservableProperty]
        private int _pageNum;

		[ObservableProperty]
		private int _currentPage = 1;

		[ObservableProperty]
		private int _pageCount = 1;

		[ObservableProperty]
        private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;

        [ObservableProperty]
        private IEnumerable<FunctionViewModel> _list;

        public FunctionListViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService contentDialogService)
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
            var t = await db.WorkerManager.GetFunctionList(WorkerId, CurrentPage);
			CurrentPage = t.CurrentPage;
			PageCount = t.PageCount;
            List = t.Items;
		}

        [RelayCommand]
        private async Task OnSearchWorker()
		{
			using UnitOfWork db = new();
			AuSuBox = await db.WorkerManager.GetWorkers();
			var t = await db.WorkerManager.GetFunctionList(WorkerId, CurrentPage);
			CurrentPage = t.CurrentPage;
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
        private async Task OnRemoveAid(FucntionDetails parameter)
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
                var isSuccess = await db.WorkerManager.DeleteFunc(parameter.WorkerId, parameter.persianYear, parameter.persianMonth, parameter.Id);
                if (!isSuccess.isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", isSuccess.error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                await OnSearchWorker();
            }
        }

        [RelayCommand]
        private async Task OnUpdateFunc(FucntionDetails parameter)
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            Type? pageType = NameToPageTypeConverter.Convert("UpdateFunction");

            if (pageType == null)
            {
                return;
            }
            var service = _navigationService.GetNavigationControl();

            var func = List.First(t => t.Details.Id == parameter.Id);
            using UnitOfWork db = new();
            var list = await db.WorkerManager.GetFunctionList(WorkerId, CurrentPage);

            var context = new UpdateFunctionPage(new UpdateFunctionViewModel(_navigationService, _snackbarService)
            {
                WorkerId = parameter.WorkerId,
                AmountOf = func.Amountof,
                Description = func.Description,
                PersonnelName = func.Name,
                OverTime = func.OverTime,
                FuncId = parameter.Id,
                SubmitMonth = func.PersianMonth,
                SubmitYear = func.PersianYear,
                List = list.Items,
                PersonnelId = func.PersonelId
            });

            service.Navigate(pageType, context);
        }

		[RelayCommand]
		private async Task OnPageChenge()
		{
			using UnitOfWork db = new();
			var t = await db.WorkerManager.GetFunctionList(WorkerId, CurrentPage);
			PageCount = t.PageCount;
			List = t.Items;
		}
	}
}
