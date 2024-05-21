using Domain.NovinEntity.Workers;
using DomainShared.Constants;
using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class ProfitOrLessViewModel : ObservableObject, INavigationAware
    {

        private bool _isInitialized = false;

        [ObservableProperty]
        private string? _grossProfit;

        [ObservableProperty]
        private string? _netProfit;

        [ObservableProperty]
        private string? _exp;

        [ObservableProperty]
        private string? _salary;

        [ObservableProperty]
        private string? _inv;

        [ObservableProperty]
        private string? _totalSell;

        [ObservableProperty]
        private string? _totalBuy;

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                await InitializeViewModel();
        }

        private async Task InitializeViewModel()
        {
            using (UnitOfWork db = new())
            {
                var p = await db.DocumentManager.GetProfitandLossStatement();
                var exp = await db.ExpenseManager.GetTotalExp();
                var salary = await db.WorkerManager.GetTotalSalary();
                GrossProfit = p.ProfitLossStatement.ToString("N0");
                TotalBuy = p.TotalBuy.ToString("N0");
                TotalSell = p.TotalSell.ToString("N0");
                Inv = p.Inventory.ToString("N0");
                Exp = exp.ToString("N0");
                Salary = salary.ToString("N0");
                NetProfit = (p.ProfitLossStatement - (exp + salary)).ToString("N0");
            }

            _isInitialized = true;
        }
    }
}
