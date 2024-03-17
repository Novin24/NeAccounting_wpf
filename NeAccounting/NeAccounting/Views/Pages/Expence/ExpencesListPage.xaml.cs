using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for ExpensesListPage.xaml
    /// </summary>
    public partial class ExpencesListPage : Page
    {
        private readonly IContentDialogService _contentDialogService;
        public MaterailListViewModel ViewModel { get; }
        public ExpencesListPage(IContentDialogService contentDialogService, MaterailListViewModel viewModel)
        {
            _contentDialogService = contentDialogService;
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            date.Focus();
        }
    }
}
