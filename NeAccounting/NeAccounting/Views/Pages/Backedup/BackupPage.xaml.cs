using DomainShared.Extension;
using NeAccounting.Models;
using System.Globalization;
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
            BindFiles();
            SetName();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_Address.Text = dialog.SelectedPath;
            }
        }

        private void BindFiles()
        {
            PersianCalendar pc = new();
            try
            {
                List<BackFilesDetails> myFiles = [];
                DirectoryInfo d = new(Environment.CurrentDirectory + @"\BackUp\");
                FileInfo[] Files = d.GetFiles("*.bak");
                foreach (FileInfo file in Files)
                {
                    BackFilesDetails Myfile = new()
                    {
                        Id = Guid.NewGuid(),
                        FileName = file.Name,
                        FulePath = file.DirectoryName,
                        CreationTime = file.CreationTime.ToShamsiDate(pc)
                    };
                    myFiles.Add(Myfile);
                }

                if (txt_Address.Text != "")
                {
                    DirectoryInfo Md = new($@"{txt_Address.Text}");
                    FileInfo[] MFiles = Md.GetFiles("*.bak");
                    foreach (FileInfo file in MFiles)
                    {
                        BackFilesDetails nMyfile = new()
                        {
                            Id = Guid.NewGuid(),
                            FileName = file.Name,
                            CreationTime = file.CreationTime.ToShamsiDate(pc),
                            FulePath = file.FullName
                        };
                        myFiles.Add(nMyfile);
                    }

                }

                ViewModel.BakFiles = myFiles;
            }
            catch (Exception ex)
            {
                _snackbarService.Show("خطا", ex.Message, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(8000));
            }
        }

        private void SetName()
        {
            PersianCalendar pc = new();
            var time = DateTime.Now.ToShamsiDate(pc, '^');
            txt_name.Text = "BackUpDb_" + Guid.NewGuid().ToString().Replace("-", "")[..15] + "&" + time + ".bak";
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            if (btn.Tag == null)
                return;



            Guid id = Guid.Parse(btn.Tag.ToString());
        }
    }
}
