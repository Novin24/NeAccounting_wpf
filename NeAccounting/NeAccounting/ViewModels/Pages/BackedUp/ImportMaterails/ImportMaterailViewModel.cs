using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        throw new NotImplementedException();
    }
}
