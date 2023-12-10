using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// صفحه نمایش کارگران
    /// </summary>
    public partial class WorkersListPage : INavigableView<WorkerListViewModel>
    {
        public WorkerListViewModel ViewModel { get; }
        public WorkersListPage(WorkerListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
        public class Listofworkers
        {
            public string personnel_ID { set; get; }
            public string Name_and_surname_of_the_worker { set; get; }
            public string job_title { set; get; }
            public string National_Code { set; get; }
            public string Condition { set; get; }

        }


    }
}
