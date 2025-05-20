using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateFunctionPage.xaml
    /// </summary>
    public partial class UpdateFunctionPage : INavigableView<UpdateFunctionViewModel>
    {
        /// <summary>
        /// کنترل میکند ک فقط یک بار چک کند فیش حقوقی دارد یا خیر
        /// </summary>
        private bool _isInitialized;

        public UpdateFunctionViewModel ViewModel { get; }
        public UpdateFunctionPage(UpdateFunctionViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            txt_sugName.Focus();
            var limit = viewModel.FunctionLimit;
            var has = viewModel.HasSalary;
            if (!_isInitialized)
            {
                _isInitialized = true;
                
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is UpdateFunctionPage c)
            {
                c.ViewModel.CheckAndShowSnackbar();
                //txt_Name.SetCurrentValue(AutoSuggestBox.TextProperty, c.ViewModel.CusName);
            }
            
        }
    }
}
