using DomainShared.Enums;
using DomainShared.ViewModels.Customer;
using DomainShared.ViewModels.Pun;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

public partial class ImportMaterailViewModel : ObservableObject, INavigationAware
{
    /// <summary>
    /// ادرس فایل
    /// </summary>
    [ObservableProperty]
    private string _exPaht;


    /// <summary>
    /// نام فایل
    /// </summary>
    [ObservableProperty]
    private string _fileName;

    public void OnNavigatedFrom()
    {
        
    }

    public void OnNavigatedTo()
    {
    }

}
