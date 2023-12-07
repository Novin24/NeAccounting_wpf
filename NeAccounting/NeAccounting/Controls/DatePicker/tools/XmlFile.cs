using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NeAcconting.Controls.DatePicker
{

    public partial class ShamsiDatePicker
    {
        string XMLFileName = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\XML_DataBase.XML";
        XDocument xDocument;

        void LoadXMLFile()
        {
            if (File.Exists(XMLFileName))
                xDocument = XDocument.Load(XMLFileName);
            else
            {
                xDocument = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("\nThis XML file was created by WPF Calendar software \nPlease don't edit it manually\n"),
                    new XElement("WPF_Calendar",
                        new XElement("PERSIAN"),
                        new XElement("HIJRI")));
                xDocument.Save(XMLFileName);
            }
        }

        bool SearchInCalendar(int year, int month, int day, string whichCalendar)
        {
            bool result = false;
            try
            {
                IEnumerable<XElement> entries = from c in xDocument.Descendants(whichCalendar)
                                                from q in c.Descendants("MEMORIAL")
                                                where (q.Attribute("YEAR").Value == year.ToString() && q.Attribute("MONTH").Value == month.ToString() && q.Attribute("DAY").Value == day.ToString()) || (q.Attribute("ANNIVERSARY").Value == "True" && q.Attribute("MONTH").Value == month.ToString() && q.Attribute("DAY").Value == day.ToString())
                                                select q;
                if (entries.Count() >= 1) result = true;
            }
            catch { result = false; }
            return result;
        }

        /// <summary>
        /// Whether the input date is a holiday or not
        /// </summary>
        /// <param name="whichCalendar">Type of calendar => "HIJRI", "CHRISTIAN", "PERSIAN"</param>
        /// <returns>If the date was a holiday return True else return false</returns>
        bool isHoliday(int year, int month, int day, string whichCalendar)
        {
            bool isHoliday = false;
            try
            {
                IEnumerable<XElement> entries = from c in xDocument.Descendants(whichCalendar)
                                                from q in c.Descendants("MEMORIAL")
                                                where q.Attribute("YEAR").Value == year.ToString() && q.Attribute("MONTH").Value == month.ToString() && q.Attribute("DAY").Value == day.ToString() && q.Attribute("HOLIDAY").Value == "True"
                                                select q;

                if (entries.Count() >= 1) isHoliday = true;
            }
            catch { isHoliday = false; }
            return isHoliday;
        }

        /// <summary>
        /// whether the input date is an anniersary or not
        /// </summary>
        /// <param name="whichCalendar">Type of calendar => "HIJRI", "CHRISTIAN", "PERSIAN"</param>
        /// <returns>If the date was an anniersary return True else return false</returns>
        bool isAnniersary(int year, int month, int day, string whichCalendar)
        {
            bool IsAnniversary = false;
            try
            {
                IEnumerable<XElement> entries = from c in xDocument.Descendants(whichCalendar)
                                                from q in c.Descendants("MEMORIAL")
                                                where q.Attribute("YEAR").Value == year.ToString() && q.Attribute("MONTH").Value == month.ToString() && q.Attribute("DAY").Value == day.ToString() && q.Attribute("ANNIVERSARY").Value == "True"
                                                select q;

                if (entries.Count() >= 1) IsAnniversary = true;
            }
            catch { IsAnniversary = false; }
            return IsAnniversary;
        }

        /// <summary>
        /// Gets the content of `CONTENT` attribute from `whichCalendar` table
        /// </summary>
        /// <param name="whichCalendar">Type of calendar => "HIJRI", "CHRISTIAN", "PERSIAN"</param>
        /// <returns>content of `text` field</returns>
        string GetTextOfMemo(int year, int month, int day, string whichCalendar)
        {
            string text = "";
            try
            {
                text = (from c in xDocument.Descendants(whichCalendar)
                        from q in c.Descendants("MEMORIAL")
                        where (q.Attribute("YEAR").Value == year.ToString() && q.Attribute("MONTH").Value == month.ToString() && q.Attribute("DAY").Value == day.ToString()) || (q.Attribute("ANNIVERSARY").Value == "True" && q.Attribute("MONTH").Value == month.ToString() && q.Attribute("DAY").Value == day.ToString())
                        select q).First().Attribute("CONTENT").Value;

                return System.Web.HttpUtility.HtmlDecode(text);
            }
            catch { return ""; }
        }
    }
}
