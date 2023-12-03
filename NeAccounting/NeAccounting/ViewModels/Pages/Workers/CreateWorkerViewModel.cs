using DomainShared.Enums;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class CreateWorkerViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private string _fullName = "";

        [ObservableProperty]
        private string _jobTitle = "";

        [ObservableProperty]
        private string _mobile = "";

        [ObservableProperty]
        private string _address = "";

        [ObservableProperty]
        private DateOnly _startDate = DateOnly.FromDateTime(DateTime.Now);

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

        [ObservableProperty]
        private string _erroreMessage = "";

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo()
        {

        }

        [RelayCommand]
        private async Task OnCreate()
        {

            if (string.IsNullOrEmpty(FullName))
            {
                ErroreMessage = NeErrorCodes.IsMandatory("نام کارگر");
                return;
            }
            if (string.IsNullOrEmpty(JobTitle))
            {
                ErroreMessage = NeErrorCodes.IsMandatory("عنوان شغل");
                return;
            }
            if (string.IsNullOrEmpty(Mobile))
            {
                ErroreMessage = NeErrorCodes.IsMandatory("موبایل");
                return;
            }
            if (string.IsNullOrEmpty(Address))
            {
                ErroreMessage = NeErrorCodes.IsMandatory("آدرس");
                return;
            }
            if (PersonalId <= 0)
            {
                ErroreMessage = NeErrorCodes.IsMore("شماره پرسنلی","صفر");
                return;
            }
            if (string.IsNullOrEmpty(AccountNumber))
            {
                ErroreMessage = NeErrorCodes.IsMandatory("شماره حساب");
                return;
            }
            if (string.IsNullOrEmpty(NationalCode))
            {
                ErroreMessage = NeErrorCodes.IsMandatory("شماره ملی");
                return;
            }
            
            using UnitOfWork db = new();
            await db.workerManager.Create(
                       FullName,
                       NationalCode,
                       Mobile,
                       Address,
                       StartDate,
                       PersonalId,
                       AccountNumber,
                       Description,
                       JobTitle,
                       Shift);
        }
    }
}
