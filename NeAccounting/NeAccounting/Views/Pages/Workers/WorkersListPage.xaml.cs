using Domain.NovinEntity.Workers;
using NeAccounting.ViewModels;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO.Pipes;
using System.Windows.Media;
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

            var converter = new BrushConverter();
            ObservableCollection<Listofworkers> listofworkers =
            [
                new Listofworkers { personnel_ID = "1111", Name_and_surname_of_the_worker = "حسن احمدی", job_title = "سرکارگر", National_Code = "1090576452", Condition = "اخراج شده" },
                new Listofworkers { personnel_ID = "2222", Name_and_surname_of_the_worker = "حسین اصدی", job_title = "دستگاه", National_Code = "1090576452", Condition = "مشغول" },
                new Listofworkers { personnel_ID = "3333", Name_and_surname_of_the_worker = "حمید امدی", job_title = "ساده", National_Code = "1090576452", Condition = "اخراج شده" },
                new Listofworkers { personnel_ID = "4444", Name_and_surname_of_the_worker = "محسن رضایی", job_title = "سرشیفت", National_Code = "1090576452", Condition = "تسویه" },
                new Listofworkers { personnel_ID = "5555", Name_and_surname_of_the_worker = "علی عباسی", job_title = "بیکار", National_Code = "1090576452", Condition = "مشغول " },
            ];

            Workersdata.ItemsSource = listofworkers;


            //Listofworkers workersone = new Listofworkers();
            //workersone.personnel_ID = "1111";
            //workersone.Name_and_surname_of_the_worker = "حسن احمدی";
            //workersone.job_title = "سرکارگر";
            //workersone.National_Code = "1090576452";
            //workersone.Condition = "اخراج شده";
            //Workersdata.Items.Add(workersone);
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
