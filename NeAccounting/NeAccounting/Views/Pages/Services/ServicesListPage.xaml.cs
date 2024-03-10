using Domain.NovinEntity.Services;
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
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for ServicesListPage.xaml
    /// </summary>
    public partial class ServicesListPage : INavigableView<ServiceViewModel>
    {
        private readonly ISnackbarService _snackbarService;
        public ServiceViewModel ViewModel { get; }
        public ServicesListPage(ServiceViewModel viewModel,ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
        private void CheckBox_Status_Chkecked(object sender , RoutedEventArgs e)
        {
            if(sender is not System.Windows.Controls.CheckBox btn) 
                return;

            if (btn.Tag == null)
                return;

            int id = int.Parse(btn.Tag.ToString());
            var Srvic = ViewModel.List.First(x => x.Id == id);
            if (Srvic.IsActive)
            {
                return;
            }
            ViewModel.ActiveCommand.ExecuteAsync(id);
        }

        private void CheckBox_Status_Unckecked(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.CheckBox btn)
                return;

            if (btn.Tag == null)
                return;

            int id = int.Parse(btn.Tag.ToString());
            var Srvic = ViewModel.List.First(x => x.Id == id);
            if (!Srvic.IsActive)
            {
                return;
            }
        }
    }
}
