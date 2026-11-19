using Microsoft.Maui.Controls;
using MVVMBBasic.ViewModels;

namespace MVVMBBasic.Views;

public partial class CalculatorPage : ContentPage
{
    public CalculatorPage()
        : this(new CalculatorViewModel())
    {
    }

    public CalculatorPage(CalculatorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
