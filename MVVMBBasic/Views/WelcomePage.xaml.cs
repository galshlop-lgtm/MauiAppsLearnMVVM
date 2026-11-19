using Microsoft.Maui.Controls;
using MVVMBBasic.ViewModels;

namespace MVVMBBasic.Views;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
        BindingContext = new WelcomeViewModel();
    }
}