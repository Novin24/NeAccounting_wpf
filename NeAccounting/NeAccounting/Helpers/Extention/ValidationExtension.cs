using System.Text.RegularExpressions;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Helpers.Extention
{
    public static partial class ValidationExtension
    {
        public static bool ValidNationalCode(this string code, ISnackbarService _snackbarService)
        {
            code = code.Trim();
            Regex regex = NumOnly();
            if (string.IsNullOrEmpty(code) || code.Length != 10 || !regex.IsMatch(code))
            {
                _snackbarService.Show("کد ملی نامعتبر", "لطفا یک عدد 10 رقمی وارد کنید !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return false;
            }
            try
            {
                char[] chArray = code.ToCharArray();
                int[] numArray = new int[chArray.Length];
                for (int i = 0; i < chArray.Length; i++)
                {
                    numArray[i] = (int)char.GetNumericValue(chArray[i]);
                }
                int num2 = numArray[9];
                switch (code)
                {
                    case "0000000000":
                    case "1111111111":
                    case "22222222222":
                    case "33333333333":
                    case "4444444444":
                    case "5555555555":
                    case "6666666666":
                    case "8888888888":
                    case "9999999999":
                        _snackbarService.Show("خطا", "کد ملی وارد شده صحیح نمی باشد !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        //MessageBox.Show("کد ملی وارد شده صحیح نمی باشد");
                        break;
                }
                int num3 = ((((((((numArray[0] * 10) + (numArray[1] * 9)) + (numArray[2] * 8)) + (numArray[3] * 7)) + (numArray[4] * 6)) + (numArray[5] * 5)) + (numArray[6] * 4)) + (numArray[7] * 3)) + (numArray[8] * 2);
                int num4 = num3 - ((num3 / 11) * 11);
                if ((((num4 == 0) && (num2 == num4)) || ((num4 == 1) && (num2 == 1))) || ((num4 > 1) && (num2 == Math.Abs((int)(num4 - 11)))))
                {
                    return true;
                }
                else
                {
                    _snackbarService.Show("خطا", "کد ملی نامعتبر است !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    //MessageBox.Show("کد ملی نامعتبر است");
                }
            }
            catch (Exception)
            {
                _snackbarService.Show("خطا", "کد ملی وارد شده صحیح نمی باشد !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            }
            return false;
        }

        public static bool ValidMobileNumber(this string mobile)
        {
            Regex regex = MobileNumberOnly();
            if (regex.IsMatch(mobile))
            { 
                return true;
            }
            return false;
        }

        [GeneratedRegex(@"^\d+$")]
        private static partial Regex NumOnly();

        [GeneratedRegex(@"^(?:0|98|\+98|\+980|0098|098|00980)?(9\d{9})$")]
        private static partial Regex MobileNumberOnly();
    }
}
