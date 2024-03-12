using Domain.NovinEntity.Materials;
using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using Microsoft.Identity.Client.NativeInterop;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class CreateExpenceViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject
    {
        private readonly INavigationService _navigationService = navigationService;
        private readonly ISnackbarService _snackbarService = snackbarService;

        [ObservableProperty]
        private DateTime? _submitDate;

        [ObservableProperty]
        private string _expensetype;

        [ObservableProperty]
        private long? _amount=0;

        [ObservableProperty]
        private PaymentType _payType;

        [ObservableProperty]
        private string _receiver;

        [ObservableProperty]
        private string _description;


        [RelayCommand]
        private async Task OnCreateExpense()
        {
            if (SubmitDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if(string.IsNullOrEmpty(Expensetype))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نوع هزینه"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (Amount == null || Amount == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(Receiver))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام دریافت کننده"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.ExpenseManager.CreateExpense(SubmitDate.Value, Expensetype, Amount.Value, PayType, Receiver, Description);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                await db.SaveChangesAsync();
            }
            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

            SubmitDate = null;
            Expensetype = string.Empty;
            Receiver = string.Empty;
            Description = string.Empty;
            Amount = 0;
            PayType = PaymentType.CardToCard;
        }

    }
}
