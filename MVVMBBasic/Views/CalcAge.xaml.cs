using Microsoft.Maui.Controls;
using MVVMBBasic.ViewModels;

namespace MVVMBBasic.Views;

public partial class CalcAgePage : ContentPage
{
    public CalcAgePage() : this(new CalcAgeViewModel())
    {
    }

    public CalcAgePage(CalcAgeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
