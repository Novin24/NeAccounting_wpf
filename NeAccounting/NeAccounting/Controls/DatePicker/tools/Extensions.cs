using System.Text.RegularExpressions;

namespace NeAccounting.Controls
{
    public static class Extensions
    {
        /// <summary>
        /// متدی برای تبدیل اعداد انگلیسی به فارسی
        /// </summary>
        //public static string convertToPersianNumber(this int input)
        //{
        //    //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
        //    string temp = input.ToString();

        //    temp = Regex.Replace(temp, "0", "۰");
        //    temp = Regex.Replace(temp, "1", "۱");
        //    temp = Regex.Replace(temp, "2", "۲");
        //    temp = Regex.Replace(temp, "3", "۳");
        //    temp = Regex.Replace(temp, "4", "۴");
        //    temp = Regex.Replace(temp, "5", "۵");
        //    temp = Regex.Replace(temp, "6", "۶");
        //    temp = Regex.Replace(temp, "7", "۷");
        //    temp = Regex.Replace(temp, "8", "۸");
        //    temp = Regex.Replace(temp, "9", "۹");

        //    return temp;
        //}

        /// <summary>
        /// تبدیل اعداد فارسی به معادلش به صورت عدد integer
        /// </summary>
        public static int convertToInteger(this string input)
        {
            input = Regex.Replace(input, "۰", "0");
            input = Regex.Replace(input, "۱", "1");
            input = Regex.Replace(input, "۲", "2");
            input = Regex.Replace(input, "۳", "3");
            input = Regex.Replace(input, "۴", "4");
            input = Regex.Replace(input, "۵", "5");
            input = Regex.Replace(input, "۶", "6");
            input = Regex.Replace(input, "۷", "7");
            input = Regex.Replace(input, "۸", "8");
            input = Regex.Replace(input, "۹", "9");

            input = Regex.Replace(input, @"\D*", "");

            return int.Parse(input);
        }

        /// <summary>
        /// تبدیل نام روزهای هفته میلادی به شمسی
        /// </summary>
        public static string convertToPersianDay(this string input)
        {
            switch (input)
            {
                case "Saturday":
                    return "شنبه";

                case "Sunday":
                    return "یک شنبه";

                case "Monday":
                    return "دو شنبه";

                case "Tuesday":
                    return "سه شنبه";

                case "Wednesday":
                    return "چهار شنبه";

                case "Thursday":
                    return "پنج شنبه";

                case "Friday":
                    return "جمعه";
            }
            return "خطا";
        }

        /// <summary>
        /// تبدیل عدد ماه به معادل نام ماه شمسی
        /// </summary>
        public static string convertToPersianMonth(this int input)
        {
            string FarsiMonthName = "هیچ کدام";
            //تعیین نام ماه شمسی  
            switch (input)
            {
                //فروردین
                case 01:
                    FarsiMonthName = "فروردین";
                    break;

                //اردیبهشت
                case 02:
                    FarsiMonthName = "اردیبهشت";
                    break;

                //خرداد
                case 03:
                    FarsiMonthName = "خرداد";
                    break;

                //تیر
                case 04:
                    FarsiMonthName = "تیر";
                    break;

                //مرداد
                case 05:
                    FarsiMonthName = "مرداد";
                    break;

                //شهریور
                case 06:
                    FarsiMonthName = "شهریور";
                    break;

                //مهر
                case 07:
                    FarsiMonthName = "مهر";
                    break;

                //آبان
                case 08:
                    FarsiMonthName = "آبان";
                    break;

                //آذر
                case 09:
                    FarsiMonthName = "آذر";
                    break;

                //دی
                case 10:
                    FarsiMonthName = "دی";
                    break;

                //بهمن
                case 11:
                    FarsiMonthName = "بهمن";
                    break;

                //اسفند
                case 12:
                    FarsiMonthName = "اسفند";
                    break;
            }

            return FarsiMonthName;
        }


        /// <summary>
        /// تبدیل عدد ماه به معادل نام ماه قمری
        /// </summary>
        public static string convertToHigriMonth(this int input)
        {
            string higriMonthName = "هیچ کدام";
            //تعیین نام ماه هجری قمری  
            switch (input)
            {
                case 01:
                    higriMonthName = "محرم";
                    break;

                case 02:
                    higriMonthName = "صفر";
                    break;

                case 03:
                    higriMonthName = "ربیع الاول";
                    break;

                case 04:
                    higriMonthName = "ربیع الثانی";
                    break;

                case 05:
                    higriMonthName = "جمادی الاول";
                    break;

                case 06:
                    higriMonthName = "جمادی الثانی";
                    break;

                case 07:
                    higriMonthName = "رجب";
                    break;

                case 08:
                    higriMonthName = "شعبان";
                    break;

                case 09:
                    higriMonthName = "رمضان";
                    break;

                case 10:
                    higriMonthName = "شوال";
                    break;

                case 11:
                    higriMonthName = "ذیقعده";
                    break;

                case 12:
                    higriMonthName = "ذیحجه";
                    break;
            }

            return higriMonthName;
        }
    }
}
