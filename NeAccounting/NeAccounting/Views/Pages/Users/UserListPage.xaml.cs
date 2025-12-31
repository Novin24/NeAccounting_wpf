using NeAccounting.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages.Users
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserListPage : INavigableView<UserListViewModel>
    {
        private readonly IContentDialogService _contentDialogService;
        public UserListViewModel ViewModel { get; }
        public UserListPage(IContentDialogService contentDialogService, UserListViewModel viewModel)
        {
            ViewModel = viewModel;
            _contentDialogService = contentDialogService;
            DataContext = this;
            InitializeComponent();
        }
        private void CheckBox_Status_Chkecked(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.CheckBox btn)
                return;

            if (btn.Tag == null)
                return;

            Guid id = Guid.Parse(btn.Tag.ToString());
            var unit = ViewModel.List.First(x => x.Id == id);
            if (unit.IsActive)
            {
                return;
            }
            //ViewModel.ActiveCommand.ExecuteAsync(id);
        }
        private async void CheckBox_Status_Unckecked(object sender, RoutedEventArgs e)
        {

            if (sender is not System.Windows.Controls.CheckBox chb)
                return;
            chb.IsChecked = true;

            if (chb.Tag == null)
                return;

            Guid id = Guid.Parse(chb.Tag.ToString());
            var unit = ViewModel.List.First(x => x.Id == id);
            if (!unit.IsActive)
            {
                return;
            }
            var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
            {
                Title = "آیا از بایگانی اطمینان دارید!!!",
                Content = Application.Current.Resources["DeleteDialogContent"],
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });

            if (result == ContentDialogResult.Primary)
            {
                //await ViewModel.DeActiveCommand.ExecuteAsync(id);
            }
        }
    }
}
