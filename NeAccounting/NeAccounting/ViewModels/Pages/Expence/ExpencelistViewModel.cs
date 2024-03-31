using DomainShared.Enums;
using DomainShared.Utilities;
using DomainShared.ViewModels.Expense;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class ExpencelistViewModel : ObservableObject, INavigationAware

    {
        private bool _isInit;

        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;
        public ExpencelistViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
        }

        [ObservableProperty]
        private int _pageCount = 1;

        [ObservableProperty]
        private int _currentPage = 1;

        [ObservableProperty]
        private DateTime? _startdate;

        [ObservableProperty]
        private DateTime? _enddate;

        [ObservableProperty]
        private IEnumerable<ExpenselistDto> _list;
        public void OnNavigatedFrom()
        {
        }
        public async void OnNavigatedTo()
        {
            await InitializeViewModel();
        }

        [RelayCommand]
        private async Task InitializeViewModel()
        {
            _isInit = true;
            using UnitOfWork db = new();
            var t = await db.ExpenseManager.GetExpenselist(Startdate, Enddate, _isInit, CurrentPage);
            CurrentPage = t.CurrentPage;
            List = t.Items;
            PageCount = t.PageCount;
            _isInit = false;
        }

        [RelayCommand]
        public async Task OnSearchExpense()
        {
            _isInit = true;
            using UnitOfWork db = new();
            var t = await db.ExpenseManager.GetExpenselist(Startdate, Enddate, _isInit, CurrentPage);
            CurrentPage = t.CurrentPage;
            List = t.Items;
            PageCount = t.PageCount;
            _isInit = false;
        }

        [RelayCommand]
        private async Task OnChangePage()
        {
            if (_isInit)
            {
                return;
            }

            using UnitOfWork db = new();
            var t = await db.ExpenseManager.GetExpenselist(Startdate, Enddate, _isInit, CurrentPage);
            List = t.Items;
            PageCount = t.PageCount;
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

        [RelayCommand]
        private void OnUpdate(Guid parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("UpdateExpence");

            if (pageType == null)
            {
                return;
            }
            var ex = List.First(t => t.Id == parameter);

            var context = new UpdateExpencePage(new UpdateExpenceViewModel(_snackbarService, _navigationService)
            {
                SubmitDate = ex.Date,
                Amount = ex.Price,
                Description = ex.Description,   
                ExpenseID = ex.Id,
                Expensetype = ex.Expensetype,
                PayTypeEnum = PaymentType.Cash.ToEnumDictionary(),
                PayTypeId = ex.Type,
                Receiver = ex.Receiver,
        });

            var servise = _navigationService.GetNavigationControl();
            servise.Navigate(pageType, context);
        }

        [RelayCommand]
        private async Task OnRemove(Guid parameter)
        {
            var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
            {
                Title = "هشدار !!!",
                Content = new TextBlock() { Text = "آیا از حذف هزینه اطمینان دارید ؟", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });

            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            using UnitOfWork db = new();
            var r = await db.ExpenseManager.DeleteAsync<Guid>(parameter);

            if (!r)
            {
                _snackbarService.Show("خطا", "", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            }

            await db.SaveChangesAsync();
            _snackbarService.Show("کاربر گرامی", $"حذف هزینه با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            var t = await db.ExpenseManager.GetExpenselist(Startdate, Enddate, _isInit, CurrentPage);
            List = t.Items;
            PageCount = t.PageCount;
            return;

        }
    }
}
