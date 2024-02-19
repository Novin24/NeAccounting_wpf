using System.Windows.Controls;
using System.Windows.Input;

namespace NeAccounting.Controls
{
    /// <summary>
    /// Interaction logic for Pagination.xaml
    /// </summary>
    public partial class Pagination : UserControl
    {

        #region ctor
        private static int pageCount = 1;
        private static int currentPage = 1;
        public Pagination()
        {
            InitializeComponent();
        }
        #endregion

        #region event
        protected virtual void InitialPage(int pageCount, int currentPage)
        {
            CalculatePages(PageCount, currentPage);
        }

        private void CalculatePages(int pageCount, int currentPage)
        {
            stk_btns.Children.Clear();
            if (currentPage > 1)
            {
                Button btn = new()
                {
                    Tag = currentPage - 1,
                    Padding = new Thickness(6),
                    Margin = new Thickness(0, 0, 4, 0),
                    Content = "<"
                    //Content = Wpf.Ui.Controls.SymbolIcon.MouseLeftButtonDownEvent
                };
                btn.Click += ChagePage;
                stk_btns.Children.Add(btn);
            }

            for (int i = 1; i < pageCount + 1; i++)
            {
                if (pageCount >= 5 && i == currentPage - 3)
                {
                    Button btn = new()
                    {
                        IsEnabled = false,
                        Content = "..."
                    };
                    stk_btns.Children.Add(btn);
                    continue;
                }
                else if (i == currentPage)
                {
                    if (pageCount > 10)
                    {
                        Wpf.Ui.Controls.NumberBox number = new()
                        {
                            SpinButtonPlacementMode = Wpf.Ui.Controls.NumberBoxSpinButtonPlacementMode.Hidden,
                            ClearButtonEnabled = false,
                            Padding = new Thickness(8, 6, 8, 6),
                            Value = i
                        };
                        number.KeyDown += OnEnter;
                        stk_btns.Children.Add(number);
                    }
                    else
                    {
                        Button btn = new()
                        {
                            IsEnabled = false,
                            Content = i,
                            Margin = new Thickness(2)
                        };
                        stk_btns.Children.Add(btn);
                    }

                    continue;
                }
                else if (i < currentPage + 3 && i > currentPage - 3)
                {
                    Button btn = new()
                    {
                        Content = i,
                        Tag = i,
                        Margin = new Thickness(2)
                    };
                    btn.Click += ChagePage;
                    stk_btns.Children.Add(btn);
                    continue;
                }
                else if (pageCount >= 5 && i == currentPage + 3)
                {
                    Button btn = new()
                    {
                        IsEnabled = false,
                        Content = "..."
                    };
                    stk_btns.Children.Add(btn);
                    continue;
                }
            }

            if (currentPage < pageCount)
            {
                Button btn = new()
                {
                    Tag = currentPage + 1,
                    Margin = new Thickness(4, 0, 0, 0),
                    Padding = new Thickness(6),
                    Content = ">"
                    //Content = Wpf.Ui.Controls.SymbolIcon.MouseLeftButtonDownEvent
                };
                btn.Click += ChagePage;
                stk_btns.Children.Add(btn);
            }
        }

        private void OnEnter(object sender, KeyEventArgs e)
        {
            if (sender is not Wpf.Ui.Controls.NumberBox txt)
                return;

            if (!txt.Value.HasValue)
            {
                return;
            }

            int selecterPage = (int)txt.Value;
            if (selecterPage <= 0)
            {
                selecterPage = 1;
            }

            if (selecterPage > PageCount)
            {
                selecterPage = PageCount;
            }

            if (e.Key == Key.Enter)
            {
                CurrntPage = selecterPage;
            }
        }

        private void ChagePage(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            if (btn.Tag == null)
                return;

            int id = int.Parse(btn.Tag.ToString());
            CurrntPage = id;
        }


        #endregion

        #region Propertys

        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minpage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(Pagination), new PropertyMetadata(0, CountChage));

        private static void CountChage(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is not Pagination pg)
                return;

            if (args.NewValue == args.OldValue)
                return;

            pageCount = (int)args.NewValue;
            pg.InitialPage(pageCount, currentPage);
        }

        public int CurrntPage
        {
            get { return (int)GetValue(CurrntPageProperty); }
            set { SetValue(CurrntPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maxpage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrntPageProperty =
            DependencyProperty.Register("CurrntPage", typeof(int), typeof(Pagination), new PropertyMetadata(0, PropertyChenged));
        private static void PropertyChenged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is not Pagination pg)
                return;

            if (args.NewValue == args.OldValue)
                return;
            currentPage = (int)args.NewValue;
            pg.InitialPage(pageCount, currentPage);
            RoutedPropertyChangedEventArgs<int> e = new((int)args.OldValue, (int)args.NewValue, PageChosenEvent);
            pg.OnPageChanged(e);
        }
        #endregion

        #region CustomeEvent
        /// <summary>
        /// Event occurs when the user selects an item from the recommended ones.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<int> PageChosen
        {
            add => AddHandler(PageChosenEvent, value);
            remove => RemoveHandler(PageChosenEvent, value);
        }

        /// <summary>
        /// Routed event for <see cref="PageChosen"/>.
        /// </summary>
        public static readonly RoutedEvent PageChosenEvent = EventManager.RegisterRoutedEvent(
            nameof(PageChosen),
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<int>),
            typeof(Pagination)
        );


        protected virtual void OnPageChanged(RoutedPropertyChangedEventArgs<int> args)
        {
            RaiseEvent(args);
        }
        #endregion
    }
}
