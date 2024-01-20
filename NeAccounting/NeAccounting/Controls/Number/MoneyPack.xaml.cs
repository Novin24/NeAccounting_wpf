using System.Windows.Controls;

namespace NeAccounting.Controls
{
    /// <summary>
    /// Interaction logic for MoneyPack.xaml
    /// </summary>
    public partial class MoneyPack : UserControl
    {
        public event RoutedEventHandler NumberLostFocus;
        public event TextChangedEventHandler TextChanged;
        public event RoutedEventHandler ValueChanged;
        public MoneyPack()
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
            DependencyProperty.Register("LableName", typeof(string), typeof(MoneyPack), new PropertyMetadata(string.Empty, SetLabelName));

        private static void SetLabelName(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is not MoneyPack npack)
                return;

            if (e.NewValue == e.OldValue)
                return;

            npack.lbl_name.Text = e.NewValue.ToString();
        }

        public long Value
        {
            get { return (long)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumberValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(long), typeof(MoneyPack), new PropertyMetadata(0L, SetNumber));

        private static void SetNumber(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

            if (obj is not MoneyPack npack)
                return;

            if (e.NewValue == e.OldValue)
                return;

            npack.txb_num.Text = ((long)e.NewValue).ToString("N0");
        }




        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(MoneyPack), new PropertyMetadata(double.MinValue));



        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(MoneyPack), new PropertyMetadata(double.MaxValue));



        private void NumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        private void NumberBox_ValueChanged(object sender, RoutedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        private void NumberBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NumberLostFocus?.Invoke(this, e);
        }
    }
}
