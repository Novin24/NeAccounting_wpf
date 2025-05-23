﻿using NeAccounting.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateSalaryPage.xaml
    /// </summary>
    public partial class UpdateSalaryPage : INavigableView<UpdateSalaryViewModel>
    {
        public UpdateSalaryViewModel ViewModel { get; }

        /// <summary>
        /// همه ی اضافات
        /// </summary>
        private long Additions = 0;

        /// <summary>
        /// همه ی کسورات
        /// </summary>
        private long Deductions = 0;
        private long LeftOver = 0;
        public UpdateSalaryPage(UpdateSalaryViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPageByShift();
        }

        private void SetTotalPlusPrice()
        {
            if (!IsInitialized)
            {
                return;
            }

            // همه ی اضافات
            var total = txt_amountOf.Value + txt_overtime.Value + txt_ChildAllowance.Value + txt_OtherAdditions.Value + txt_RighOfFood.Value;

            // همه ی اضافات
            Additions = total;
            LeftOver = Additions - Deductions;
            ViewModel.LeftOver = LeftOver;
            txt_totalPlus.Text = total.ToString("N0");
            lbl_leftOver.Text = Math.Abs(LeftOver).ToString("N0");
        }

        private void NumberBox_ValueChanged(object sender, RoutedEventArgs e)
        {
            var input = ((MoneyPack)sender).Value;
            if (input != 0)
            {
                SetTotalPlusPrice();
            }
        }

        private void SetTotalPrice()
        {
            if (!IsInitialized)
            {
                return;
            }
            //همه ی کسورات
            var total = txt_Aid.Value + txt_Insurance.Value + txt_Tax.Value + txt_loanInstallment.Value + txt_Othere.Value;

            //محاسبه تمام اضافات
            Additions = txt_amountOf.Value + txt_overtime.Value + txt_ChildAllowance.Value + txt_OtherAdditions.Value + txt_RighOfFood.Value;
            //همه ی کسورات
            Deductions = total;
            LeftOver = Additions - Deductions;
            ViewModel.LeftOver = LeftOver;
            txt_totalMines.Text = total.ToString("N0");
            lbl_leftOver.Text = Math.Abs(LeftOver).ToString("N0");
            if (LeftOver != 0)
            {
                if (LeftOver > 0)
                {
                    lbl_status.Text = "طلبکار";
                    lbl_status.Foreground = new SolidColorBrush(Colors.IndianRed);
                }
                else
                {
                    lbl_status.Text = "بدهکار";
                    lbl_status.Foreground = new SolidColorBrush(Colors.DarkSeaGreen);
                }

            }

        }

        private void NumberMinesBox_ValueChanged(object sender, RoutedEventArgs e)
        {
            var input = ((MoneyPack)sender).Value;

            SetTotalPrice();

        }

        private async Task ReloadSalary()
        {
            if (!await ViewModel.OnSelect())
            {
                return;
            }
            LoadPageByShift();
        }

        private async void Dtp_MonthChosen(object sender, RoutedPropertyChangedEventArgs<byte?> e)
        {
            if (!IsInitialized)
            {
                return;
            }

            //if (ViewModel.WorkerId == null)
            //    return;

            await ReloadSalary();
        }

        private async void Dtp_YearChosen(object sender, RoutedPropertyChangedEventArgs<int?> e)
        {
            if (!IsInitialized)
            {
                return;
            }
            //if (ViewModel.WorkerId == -1)
            //    return;

            await ReloadSalary();
        }

        private void LoadPageByShift()
        {
            if (DataContext is UpdateSalaryPage usp)
            {
                // اینجو رو دوباره ببین
                Deductions = txt_Aid.Value + txt_Insurance.Value + txt_Tax.Value + txt_loanInstallment.Value + txt_Othere.Value;
                Additions = txt_amountOf.Value + txt_overtime.Value + txt_ChildAllowance.Value + txt_OtherAdditions.Value + txt_RighOfFood.Value;
                if (usp.ViewModel.ShiftStatus == DomainShared.Enums.Shift.ByMounth)
                {
                    txt_Tax.IsEnabled = true;
                    txt_loanInstallment.IsEnabled = true;
                    txt_RighOfFood.IsEnabled = true;
                    txt_ChildAllowance.IsEnabled = true;
                }
                else
                {
                    txt_Tax.Value = 0;
                    txt_Tax.IsEnabled = false;

                    txt_loanInstallment.Value = 0;
                    txt_loanInstallment.IsEnabled = false;

                    txt_RighOfFood.Value = 0;
                    txt_RighOfFood.IsEnabled = false;

                    txt_ChildAllowance.Value = 0;
                    txt_ChildAllowance.IsEnabled = false;
                }
            }
        }
    }
}
