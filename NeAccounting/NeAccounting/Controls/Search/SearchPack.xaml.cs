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


        #region Lable
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

        public string LabelNumber
        {
            get { return (string)GetValue(LableNameProperty); }
            set { SetValue(LableNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LableName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LableNumberProperty =
            DependencyProperty.Register("LabelNumber", typeof(string), typeof(SearchPack), new PropertyMetadata(string.Empty, SetLabelNumber));

        private static void SetLabelNumber(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is not SearchPack npack)
                return;

            if (e.NewValue == e.OldValue)
                return;

            npack.lbl_name.Text = e.NewValue.ToString();
        }
        #endregion

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



        //public object? SelectedItem
        //{
        //    get { return (object?)GetValue(SelectedItemProperty); }
        //    set { SetValue(SelectedItemProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty SelectedItemProperty =
        //    DependencyProperty.Register("SelectedItem", typeof(object?), typeof(SearchPack), new PropertyMetadata(0));



        private void Txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SuggestionChosen?.Invoke(sender, args);
        }

        private void Dismiss_Click(object sender, RoutedEventArgs e)
        {
            lbl_name.Text = string.Empty;
            lbl_num.Text = string.Empty;
            //SelectedItem = null;

        }
    }
}
