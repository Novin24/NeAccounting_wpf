using System.Windows.Controls;

namespace NeAccounting.Controls
{
    /// <summary>
    /// Interaction logic for TextPack.xaml
    /// </summary>
    public partial class TextPack : UserControl
    {
        public event RoutedEventHandler NumberLostFocus;
        public event TextChangedEventHandler TextChanged;
        public event RoutedEventHandler ValueChanged;
        public TextPack()
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
            DependencyProperty.Register("LableName", typeof(string), typeof(TextPack), new PropertyMetadata(string.Empty, SetLabelName));

        private static void SetLabelName(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is not TextPack npack)
                return;

            if (e.NewValue == e.OldValue)
                return;

            npack.lbl_name.Text = e.NewValue.ToString();
        }

        public long Text
        {
            get { return (long)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumberValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(long), typeof(TextPack), new PropertyMetadata(string.Empty));//, SetNumber));

        private static void SetNumber(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

            if (obj is not TextPack npack)
                return;

            if (e.NewValue == e.OldValue)
                return;
        }





        private void NumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }


        private void NumberBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NumberLostFocus?.Invoke(this, e);
        }
    }
}
