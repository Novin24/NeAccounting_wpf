using DomainShared.Enums;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class UpdateWorkerViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        public UpdateWorkerViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
        }
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _fullName = "";

        [ObservableProperty]
        private string _jobTitle = "";

        [ObservableProperty]
        private string _mobile = "";

        [ObservableProperty]
        private string _address = "";

        [ObservableProperty]
        private DateTime _startDate = DateTime.Now;

        [ObservableProperty]
        private int _personalId;

        [ObservableProperty]
        private string _accountNumber = "";

        [ObservableProperty]
        private string _nationalCode = "";

        [ObservableProperty]
        private string _description = "";

        [ObservableProperty]
        private Shift _shift = Shift.ByMounth;

        [ObservableProperty]
        private Status _status = Status.InWork;


        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo()
        {

        }

        [RelayCommand]
        private async Task OnUpdate()
        {

            if (string.IsNullOrEmpty(FullName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کارگر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(JobTitle))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("عنوان شغل"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Mobile))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("موبایل"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Address))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("آدرس"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (PersonalId <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("شماره پرسنلی", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(AccountNumber))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("شماره حساب"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(NationalCode))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("شماره ملی"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            using UnitOfWork db = new();
            var (error, isSuccess) = await db.workerManager.Update(
                       Id,
                       FullName,
                       NationalCode,
                       Mobile,
                       Address,
                       StartDate,
                       PersonalId,
                       AccountNumber,
                       Description,
                       JobTitle,
                       Status,
                       Shift);

            if (!isSuccess)
            {
                _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            await db.SaveChangesAsync();


            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(2000));

            Type? pageType = NameToPageTypeConverter.Convert("WorkersList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }
    }
}
