using DomainShared.Enums;
using DomainShared.Utilities;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateNotificationPage.xaml
    /// </summary>
    public partial class CreateNotificationPage : INavigableView<CreateNotifViewModel>
    {
        public CreateNotifViewModel ViewModel { get; }

        public CreateNotificationPage(CreateNotifViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Txt_Titele.Focus();
            var t = Priority.All.ToEnumDictionary();
            t.Remove(Priority.All);
            cmb_Priority.ItemsSource = t;
        }

        [RelayCommand]
        private async Task OnSubmit()
        {
            Btn_submit.Focus();
            await ViewModel.SubmitCommand.ExecuteAsync(null);
        }
    }
}
