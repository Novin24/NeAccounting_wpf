using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Notifications;
using DomainShared.Utilities;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class NotifListViewModel : ObservableObject, INavigationAware
    {
        private bool _isInit;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;
        private bool _isreadonly = true;

        public NotifListViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
            _isreadonly = NeAccountingConstants.ReadOnlyMode;
        }

        #region Properties

        /// <summary>
        /// از تاریخ
        /// </summary>
        [ObservableProperty]
        private DateTime? _startDate;

        /// <summary>
        /// تا تاریخ
        /// </summary>
        [ObservableProperty]
        private DateTime? _endDate;

        /// <summary>
        /// <summary>
        /// عنوان
        /// </summary>
        [ObservableProperty]
        private string _titele;

        /// <summary>
        /// درجه اهمیت
        /// </summary>
        [ObservableProperty]
        private Priority _priority = Priority.All;

        /// <summary>
        /// لیست چک
        /// </summary>
        [ObservableProperty]
        private IEnumerable<NotifViewModel> _notifList;

        [ObservableProperty]
        private int _pageCount = 1;

        [ObservableProperty]
        private int _currentPage = 1;
        #endregion

        #region Methods
        public async void OnNavigatedTo()
        {
            await InitializeViewModel();
        }
        public void OnNavigatedFrom()
        {

        }

        private async Task InitializeViewModel()
        {
            _isInit = true;
            using BaseUnitOfWork db = new();
            var t = await db.NotifRepository.GetNotifs(Titele, Priority, StartDate, EndDate, _isInit, CurrentPage);
            CurrentPage = t.CurrentPage;
            NotifList = t.Items;
            PageCount = t.PageCount;
            _isInit = false;
        }

        [RelayCommand]
        private async Task OnSearch()
        {
            _isInit = true;
            using BaseUnitOfWork db = new();
            var t = await db.NotifRepository.GetNotifs(Titele, Priority, StartDate, EndDate, _isInit, CurrentPage);
            CurrentPage = t.CurrentPage;
            NotifList = t.Items;
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
            using BaseUnitOfWork db = new();
            var t = await db.NotifRepository.GetNotifs(Titele, Priority, StartDate, EndDate, _isInit, CurrentPage);
            NotifList = t.Items;
            PageCount = t.PageCount;
        }

        [RelayCommand]
        private async Task OnRemove(int parameter)
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
            {
                Title = "هشدار !!!",
                Content = new TextBlock() { Text = "آیا از حذف یادآور اطمینان دارید ؟", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });
            if (result == ContentDialogResult.Primary)
            {
                using BaseUnitOfWork db = new();
                var (e, s) = await db.NotifRepository.DeleteNotif(parameter);
                if (!s)
                {
                    _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                await db.SaveChangesAsync();
                _snackbarService.Show("کاربر گرامی", $"حذف یادآور با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
                var t = await db.NotifRepository.GetNotifs(Titele, Priority, StartDate, EndDate, _isInit, CurrentPage);
                NotifList = t.Items;
                PageCount = t.PageCount;
            }
        }

        [RelayCommand]
        private void OnUpdate(int parameter)
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            Type? pageType = NameToPageTypeConverter.Convert("UpdateNotification");

            if (pageType == null)
            {
                return;
            }
            var notif = NotifList.FirstOrDefault(t => t.Id == parameter);
            if (notif == null)
            {
                _snackbarService.Show("کاربر گرامی", $"چک مورد نظر یافت نشد!!!", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
                return;
            }
            var t = Priority.All.ToEnumDictionary();
            t.Remove(Priority.All);

            var context = new UpdateNotificationPage(new UpdateNotifViewModel(_snackbarService, _navigationService)
            {
                NotifId = notif.Id,
                DueDate = notif.DueDate,
                Message = notif.Message,
                Priority = notif.Priority,
                Titele = notif.Titele,
                EnumSource = t
                //Cheque_Number = i.Cheque_Number,
                //CusName = i.CusName,
                //Cuslist = await db.CustomerManager.GetDisplayUser(),
                //Cheque_Owner = i.Cheque_Owner,
                //CusNum = i.CusNum,
                //Description = i.Descripion,
                //DocId = i.Id,
                //DueDate = i.DueDate,
                //Status = i.Status,
                //PageName = pageName,
                //Price = i.Price,
            });

            var servise = _navigationService.GetNavigationControl();
            servise.Navigate(pageType, context);
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
        #endregion
    }
}




