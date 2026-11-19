using Microsoft.Maui.Controls;
using MVVMBBasic.ViewModels;

namespace MVVMBBasic.Views;

public partial class ChangeCalculatorPage : ContentPage
{
    public ChangeCalculatorPage()
        : this(new ChangeCalculatorViewModel())
    {
    }

    public ChangeCalculatorPage(ChangeCalculatorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
