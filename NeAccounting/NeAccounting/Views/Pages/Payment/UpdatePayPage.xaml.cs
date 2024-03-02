using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdatePayPage.xaml
    /// </summary>
    public partial class UpdatePayPage : INavigableView<UpdatePayDocViewModel>
    {
        public UpdatePayDocViewModel ViewModel { get; set; }
        public UpdatePayPage()
        {
            DataContext = this;
            InitializeComponent();
        }

        private async void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            if (await ViewModel.OnSumbit())
            {
                //aus_CusName.Text = string.Empty;
                //Cmb_PayType.SelectedIndex = 0;
                //lbl_cusId.Text = string.Empty;
                //lbl_status.Foreground = (Brush)FindResource("TextFillColorPrimaryBrush");
            }
        }

    }
}
