using Stimulsoft.Report;

namespace NeApplication.Services
{
    public class PrintServices : IPrintServices
    {
        public (string error, bool isSuccess) PrintInvoice(string uri, string listName, object list, Dictionary<string, string> dic)
        {
            try
            {
                StiReport report = new();
                report.Load(uri);
                report.Compile();
                report.RegBusinessObject(listName, list);
                foreach (var item in dic)
                {
                    report[item.Key] = item.Value;
                }
                #region ForEach On Report dataSours
                // زمانی که کامپایل میکردیم اینا کار نمیکنن دیگ
                //var collection = report.Dictionary;
                //foreach (StiDataSource item in collection.DataSources)
                //{
                //    if (item.Name == "Customers")
                //    {
                //        item.DataTable = list.ToList().ToDataTable();
                //    }
                //}

                //foreach (StiVariable item in collection.Variables)
                //{
                //    switch (item.Name)
                //    {
                //        case "Customer_Name":
                //            item.Value = $"({cus.UniqNumber}) _ {cus.DisplayName}";
                //            break;

                //        case "Start_Date":
                //            item.Value = $"{Dtp_Start.DisplayDate}";
                //            break;

                //        case "End_Date":
                //            item.Value = $"{Dtp_End.DisplayDate}";
                //            break;

                //        case "PrintTime":
                //            item.Value = DateTime.Now.ToShamsiDate(pc);
                //            break;

                //        case "Total_Debt":
                //            item.Value = list.Select(p => p.Bed).Sum().ToString("N0");
                //            break;

                //        case "Total_Credit":
                //            item.Value = list.Select(p => p.Bes).Sum().ToString("N0");
                //            break;

                //        case "Total_LeftOVver":
                //            item.Value = list.Last().LeftOver.ToString("N0");
                //            break;

                //        case "TotalSLeftOver":
                //            item.Value = list.Last().LeftOver.ToString().NumberToPersianString();
                //            break;

                //        case "Status":
                //            item.Value = $"{list.Last().Status}";
                //            break;

                //        default:
                //            break;
                //    }
                //}
                //report.Dictionary.Synchronize();
                //StiBusinessObject dataSource = report.Dictionary.BusinessObjects[0];
                //StiVariable vr = report.Dictionary.Variables[0];
                #endregion
                report.RenderWithWpf();
                report.ShowWithWpf();
            }
            catch (Exception ex)
            {
                return new(ex.Message, false);
            }
            return new(string.Empty, true);
        }

        public (string error, bool isSuccess) PrintInvoice(string uri, string listName, object list)
        {
            try
            {
                StiReport report = new();
                report.Load(uri);
                report.Compile();
                report.RegBusinessObject(listName, list);
                report.RenderWithWpf();
                report.ShowWithWpf();
            }
            catch (Exception ex)
            {
                return new(ex.Message, false);
            }
            return new(string.Empty, true);
        }

        public (string error, bool isSuccess) PrintInvoice(string uri, Dictionary<string, string> dic)
        {
            try
            {
                StiReport report = new();
                report.Load(uri);
                report.Compile();
                foreach (var item in dic)
                {
                    report[item.Key] = item.Value;
                }
                report.RenderWithWpf();
                report.ShowWithWpf();
            }
            catch (Exception ex)
            {
                return new(ex.Message, false);
            }
            return new(string.Empty, true);
        }

        public (string error, bool isSuccess) PrintInvoice(string uri)
        {
            try
            {
                StiReport report = new();
                report.Load(uri);
                report.Compile();
                report.RenderWithWpf();
                report.ShowWithWpf();
            }
            catch (Exception ex)
            {
                return new(ex.Message, false);
            }
            return new(string.Empty, true);
        }
    }
}
