namespace NeAccounting.Windows
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow
    {
        public LoadingWindow()
        {
            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

            InitializeComponent();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
