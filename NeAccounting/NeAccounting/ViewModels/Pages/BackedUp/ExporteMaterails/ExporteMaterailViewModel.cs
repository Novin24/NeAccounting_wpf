using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeAccounting.ViewModels;

public partial class ExporteMaterailViewModel : ObservableObject
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
}
