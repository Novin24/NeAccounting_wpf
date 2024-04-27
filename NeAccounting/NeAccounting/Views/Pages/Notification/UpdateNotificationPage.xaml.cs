using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateNotificationPage.xaml
    /// </summary>
    public partial class UpdateNotificationPage : INavigableView<UpdateNotifViewModel>
    {
        public UpdateNotifViewModel ViewModel { get; }

        public UpdateNotificationPage(UpdateNotifViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Txt_Titele.Focus();
            //var t = Priority.All.ToEnumDictionary();
            //t.Remove(Priority.All);
            //cmb_Priority.ItemsSource = t;
        }

        /// <summary>
        /// ثبت یاداور
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnSubmit()
        {
            cmb_Priority.Focus();
            //if (DataContext is not UpdateNotificationPage unp)
            //{
            //    return;
            //}
            var t =  Txt_Titele.Text;
            await ViewModel.SubmitCommand.ExecuteAsync(null);

        }
    }
}
