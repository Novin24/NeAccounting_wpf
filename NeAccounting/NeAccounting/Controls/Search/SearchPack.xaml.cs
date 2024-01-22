using System.Collections;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Controls
{
    /// <summary>
    /// Interaction logic for TextPack.xaml
    /// </summary>
    public partial class SearchPack : UserControl
    {
        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxSuggestionChosenEventArgs> SuggestionChosen;

        public SearchPack()
        {
            InitializeComponent();
        }

        public string LabelName
        {
            get { return (string)GetValue(LableNameProperty); }
            set { SetValue(LableNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LableName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LableNameProperty =
            DependencyProperty.Register("LableName", typeof(string), typeof(SearchPack), new PropertyMetadata(string.Empty, SetLabelName));

        private static void SetLabelName(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is not SearchPack npack)
                return;

            if (e.NewValue == e.OldValue)
                return;

            npack.lbl_name.Text = e.NewValue.ToString();
        }

        public IList OriginalItemsSource
        {
            get
            {
                return (IList)GetValue(OriginalItemsSourceProperty);
            }
            set
            {
                SetValue(OriginalItemsSourceProperty, value);
            }
        }

        public static readonly DependencyProperty OriginalItemsSourceProperty =
            DependencyProperty.Register("OriginalItemsSource", typeof(IList), typeof(SearchPack), new PropertyMetadata(Array.Empty<object>()));


        private void Txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SuggestionChosen?.Invoke(sender, args);
        }
    }
}
