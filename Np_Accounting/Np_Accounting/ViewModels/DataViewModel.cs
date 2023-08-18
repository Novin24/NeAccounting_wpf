using Np_Accounting.Models;
using System.Windows.Media;

namespace Np_Accounting.ViewModels
{
    public partial class DataViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private IEnumerable<DataColor> _colors;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            var random = new Random();
            var colorCollection = new List<DataColor>();

            for (int i = 0; i < 100; i++)
                colorCollection.Add(new DataColor
                {
                    Colorr = new SolidColorBrush(Color.FromArgb(
                        (byte)200,
                        (byte)random.Next(0, 250),
                        (byte)random.Next(0, 250),
                        (byte)random.Next(0, 250)))
                });

            Colors = colorCollection;

            _isInitialized = true;
        }
    }
}
