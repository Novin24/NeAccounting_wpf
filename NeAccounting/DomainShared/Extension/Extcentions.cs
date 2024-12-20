﻿using System.Data;
using System.Globalization;
using System.Reflection;

namespace DomainShared.Extension
{
    public static class Extcentions
    {
        public static string ToShamsiDate(this DateTime? date, PersianCalendar pc)
        {
            if (date == null)
                return string.Empty;
            return string.Concat(pc.GetYear(date.Value), "/", pc.GetMonth(date.Value), "/", pc.GetDayOfMonth(date.Value));
		}

		public static string ToShamsiDateNotSlash(this DateTime? date, PersianCalendar pc)
		{
			if (date == null)
				return string.Empty;

			string month = pc.GetMonth(date.Value).ToString("D2");
			string day = pc.GetDayOfMonth(date.Value).ToString("D2");
			return string.Concat(pc.GetYear(date.Value), "", month, "", day);
		}

		public static string AddNegatives(this long price)
		{
			string priceString = price.ToString();
			int length = priceString.Length;
			int negativesToAdd = Math.Max(0, 15 - length); 

			return new string('-', negativesToAdd) ; 
		}

		public static string ShamsiDateToString(this DateTime? date, PersianCalendar pc)
		{
			if (date == null)
				return string.Empty;
			string year = pc.GetYear(date.Value).ToString();
            var month = pc.GetMonth(date.Value);
			string day = pc.GetDayOfMonth(date.Value).ToString();
			
            string monthName = month switch
			{
				1 => "فروردین",
				2 => "اردیبهشت",
				3 => "خرداد",
				4 => "تیر",
				5 => "مرداد",
				6 => "شهریور",
				7 => "مهر",
				8 => "آبان",
				9 => "آذر",
				10 => "دی",
				11 => "بهمن",
				12 => "اسفند",
				_ => throw new ArgumentOutOfRangeException(nameof(month), "ماه نامعتبر است")
			};

			string dayInWords = day.NumberToPersianString();
			string yearInWords = year.NumberToPersianString();

			return $"{dayInWords} ، {monthName} ، {yearInWords}";

		}

		public static string ToShamsiDate(this DateTime date, PersianCalendar pc)
        {
            return string.Concat(pc.GetYear(date), "/", pc.GetMonth(date), "/", pc.GetDayOfMonth(date));
        }

        public static string ToShamsiDate(this DateTime date, PersianCalendar pc, char sep)
        {
            return string.Concat(pc.GetYear(date), sep, pc.GetMonth(date), sep, pc.GetDayOfMonth(date));
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

		#region NumberToPersianString
		public static string NumberToPersianString(this string TXT)
        {
            string RET = string.Empty, STRVA;
            string[] MainStr = STR_To_Int(TXT);
            int Q = 0;
            for (int i = MainStr.Length - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(MainStr[i]) == 0) { Q++; continue; }
                STRVA = string.Empty;
                if (RET != "" && RET != null)
                    STRVA = " و ";
                RET = Convert_STR(GETCountStr(MainStr[i]), Q) + STRVA + RET;
                Q++;
            }
            if (RET.Trim() == "" || RET == null)
                RET = "صفر";
            return RET;
        }

        private static string[] STR_To_Int(string STR)
        {
            STR = GETCountStr(STR);
            string[] RET = new string[STR.Length / 3];
            int Q = 0;
            for (int I = 0; I < STR.Length; I += 3)
            {
                RET[Q] = STR.Substring(I, 3);
                Q++;
            }
            return RET;
        }

        private static string GETCountStr(string STR)
        {
            string RET = STR;
            int LEN = (STR.Length / 3 + 1) * 3 - STR.Length;
            if (LEN < 3)
            {
                for (int i = 0; i < LEN; i++)
                {
                    RET = "0" + RET;
                }
            }
            if (RET == "")
                return "000";
            return RET;
        }

        private static string Convert_STR(string INT, int Count)
        {
            string RET = string.Empty;
            if (Convert.ToInt32(INT) == 0) return RET;
            //یک صد
            if (Count == 0)
            {
                if (INT.Substring(1, 1) == "1" && INT.Substring(2, 1) != "0")
                {
                    RET = GET_Number(3, Convert.ToInt32(INT.Substring(0, 1)), " ") + GET_Number(1, Convert.ToInt32(INT.Substring(2, 1)), "");
                }
                else
                {
                    string STR = GET_Number(0, Convert.ToInt32(INT.Substring(2, 1)), "");
                    RET = GET_Number(3, Convert.ToInt32(INT.Substring(0, 1)), GET_Number(2, Convert.ToInt32(INT.Substring(1, 1)), "") + STR) + GET_Number(2, Convert.ToInt32(INT.Substring(1, 1)), STR) + GET_Number(0, Convert.ToInt32(INT.Substring(2, 1)), "");
                }
            }
            //هزار
            else if (Count == 1)
            {
                RET = Convert_STR(INT, 0);
                RET += " هزار";
            }
            //میلیون
            else if (Count == 2)
            {
                RET = Convert_STR(INT, 0);
                RET += " میلیون";
            }
            //میلیارد
            else if (Count == 3)
            {
                RET = Convert_STR(INT, 0);
                RET += " میلیارد";
            }
            //میلیارد
            else if (Count == 4)
            {
                RET = Convert_STR(INT, 0);
                RET += " تیلیارد";
            }
            //میلیارد
            else if (Count == 5)
            {
                RET = Convert_STR(INT, 0);
                RET += " بیلیارد";
            }
            else
            {
                RET = Convert_STR(INT, 0);
                RET += Count.ToString();
            }
            return RET;
        }

        private static string GET_Number(int Count, int Number, string VA)
        {
            string RET = "";

            if (VA != "" && VA != null)
            {
                VA = " و ";
            }
            if (Count == 0 || Count == 1)
            {
                bool IsDah = Convert.ToBoolean(Count);
                string[] MySTR = new string[10];
                MySTR[1] = IsDah ? "یازده" : "یک" + VA;
                MySTR[2] = IsDah ? "دوازده" : "دو" + VA;
                MySTR[3] = IsDah ? "سیزده" : "سه" + VA;
                MySTR[4] = IsDah ? "چهارده" : "چهار" + VA;
                MySTR[5] = IsDah ? "پانزده" : "پنج" + VA;
                MySTR[6] = IsDah ? "شانزده" : "شش" + VA;
                MySTR[7] = IsDah ? "هفده" : "هفت" + VA;
                MySTR[8] = IsDah ? "هجده" : "هشت" + VA;
                MySTR[9] = IsDah ? "نوزده" : "نه" + VA;
                return MySTR[Number];
            }
            else if (Count == 2)
            {
                string[] MySTR = new string[10];
                MySTR[1] = "ده";
                MySTR[2] = "بیست" + VA;
                MySTR[3] = "سی" + VA;
                MySTR[4] = "چهل" + VA;
                MySTR[5] = "پنجاه" + VA;
                MySTR[6] = "شصت" + VA;
                MySTR[7] = "هفتاد" + VA;
                MySTR[8] = "هشتاد" + VA;
                MySTR[9] = "نود" + VA;
                return MySTR[Number];
            }
            else if (Count == 3)
            {
                string[] MySTR = new string[10];
                MySTR[1] = "یکصد" + VA;
                MySTR[2] = "دویست" + VA;
                MySTR[3] = "سیصد" + VA;
                MySTR[4] = "چهارصد" + VA;
                MySTR[5] = "پانصد" + VA;
                MySTR[6] = "ششصد" + VA;
                MySTR[7] = "هفتصد" + VA;
                MySTR[8] = "هشتصد" + VA;
                MySTR[9] = "نهصد" + VA;
                return MySTR[Number];
            }
            return RET;
        }
        #endregion
    }
}