namespace NeApplication.Services
{
    public interface IPrintServices
    {
        (string error, bool isSuccess) PrintInvoice(string uri, string listName, object list, Dictionary<string, string> dic);
        (string error, bool isSuccess) PrintInvoice(string uri, string listName, object list);
        (string error, bool isSuccess) PrintInvoice(string uri, Dictionary<string, string> dic);
        (string error, bool isSuccess) PrintInvoice(string uri);
    }
}
