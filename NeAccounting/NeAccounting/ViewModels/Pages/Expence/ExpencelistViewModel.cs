using DomainShared.ViewModels.Customer;
using DomainShared.ViewModels.Expense;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class ExpencelistViewModel : ObservableObject, INavigationAware

    {
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
        private DateTime _startdate;

        [ObservableProperty]
        private DateTime _enddate;

        [ObservableProperty]
        private IEnumerable<ExpenselistDto> _list;
        public void OnNavigatedFrom()
        {
        }
        public  async void OnNavigatedTo()
        {
            await InitializeViewModel();
        }
        [RelayCommand]
        private async Task InitializeViewModel()
        {
            using UnitOfWork db = new();
            List = await db.ExpenseManager.GetExpenselist(null, null);
        }
        [RelayCommand]
        public async Task OnSearchExpense()
        {
            using UnitOfWork db = new();
            List = await db.ExpenseManager.GetExpenselist(null, null);
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
        private void OnUpdateEx(Guid parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("UpdateExpence");

            if (pageType == null)
            {
                return;
            }
            var servise = _navigationService.GetNavigationControl();

            var cus = List.First(t => t.Id == parameter);
        }
    }
}
