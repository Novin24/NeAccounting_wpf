using NeAccounting.ViewModels;
using System.IO;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for Backup.xaml
    /// </summary>
    public partial class BackupPage : INavigableView<BackupViewModel>
    {
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;
        public BackupViewModel ViewModel { get; }
        public BackupPage(BackupViewModel viewModel, ISnackbarService snackbarService, IContentDialogService contentDialogService)
        {
            _snackbarService = snackbarService;
            _contentDialogService = contentDialogService;
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private async void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            if (btn.Tag == null)
                return;

            var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
            {
                Title = "هشدار !!!",
                Content = new TextBlock() { Text = "آیا از حذف فایل اطمینان دارید ؟", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });

            if (result == ContentDialogResult.Primary)
            {
                Guid id = Guid.Parse(btn.Tag.ToString());
                var file = ViewModel.BakFiles.First(x => x.Id == id).FullName;

                if (DeleteFile(file))
                {
                    _snackbarService.Show("کاربر گرامی", $"حذف فایل با موفقیت انجام شد ", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
                    ViewModel.BindFiles();
                    return;
                }

                _snackbarService.Show("خطا", "حذف فایل امکان پذیر نیست !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            }
        }

        private static bool DeleteFile(string fullName)
        {
            try
            {
                File.Delete(fullName);
                return true;

            }
            catch
            {
                return false;
            }
        }

        private void Btn_Brows_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ViewModel.ExPaht = dialog.SelectedPath;
                ViewModel.BindFiles();
            }
        }
    }
}
