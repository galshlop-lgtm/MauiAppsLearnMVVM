using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MVVMBBasic.ViewModels;

public sealed class WelcomeViewModel : BaseViewModel
{
    public WelcomeViewModel()
    {
        Title = "Welcome";
        OpenAgeCalculatorCommand = new Command(() => NavigateTo("CalcAge"));
        OpenMathCalculatorCommand = new Command(() => NavigateTo("CalculatorPage"));
        OpenChangeCalculatorCommand = new Command(() => NavigateTo("ChangeCalculator"));
    }

    public ICommand OpenAgeCalculatorCommand { get; }
    public ICommand OpenMathCalculatorCommand { get; }
    public ICommand OpenChangeCalculatorCommand { get; }

    private static void NavigateTo(string route)
    {
        if (Shell.Current is not null)
        {
            _ = Shell.Current.GoToAsync(route);
        }
    }
}
