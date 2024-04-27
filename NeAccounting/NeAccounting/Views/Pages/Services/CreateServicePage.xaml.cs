using NeAccounting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateServicePage.xaml
    /// </summary>
    public partial class CreateServicePage : INavigableView<CreateServiceViewModel>
    {
        public CreateServiceViewModel ViewModel { get; }
        public CreateServicePage(CreateServiceViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            SrvicName.Focus();

        }

        [RelayCommand]
        private async Task OnCreateService()
        {
            Btn_submit.Focus();
            await ViewModel.CreateServiceCommand.ExecuteAsync(null);
        }
    }
}
