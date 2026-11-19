using System.Globalization;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MVVMBBasic.ViewModels;

public sealed class CalculatorViewModel : BaseViewModel
{
    private string _number1 = string.Empty;
    private string _number2 = string.Empty;
    private string _result = "0";

    public CalculatorViewModel()
    {
        Title = "מחשבון";

        AddCommand = new Command(async () => await AddAsync());
        SubtractCommand = new Command(async () => await SubtractAsync());
        MultiplyCommand = new Command(async () => await MultiplyAsync());
        DivideCommand = new Command(async () => await DivideAsync());
    }

    public string Number1
    {
        get => _number1;
        set => SetProperty(ref _number1, value);
    }

    public string Number2
    {
        get => _number2;
        set => SetProperty(ref _number2, value);
    }

    public string Result
    {
        get => _result;
        private set => SetProperty(ref _result, value);
    }

    public ICommand AddCommand { get; }
    public ICommand SubtractCommand { get; }
    public ICommand MultiplyCommand { get; }
    public ICommand DivideCommand { get; }

    private async Task AddAsync()
    {
        await CalculateAsync((left, right) => left + right);
    }

    private async Task SubtractAsync()
    {
        await CalculateAsync((left, right) => left - right);
    }

    private async Task MultiplyAsync()
    {
        await CalculateAsync((left, right) => left * right);
    }

    private async Task DivideAsync()
    {
        if (!TryGetInputs(out double left, out double right))
        {
            await ShowAlertAsync(
                "קלט לא תקין",
                "אנא הכנס מספרים תקינים בשתי התיבות",
                "אישור");

            return;
        }

        if (right == 0)
        {
            await ShowAlertAsync(
                "שגיאה",
                "לא ניתן לחלק באפס!",
                "אישור");

            Result = "שגיאה";
            return;
        }

        Result = (left / right).ToString(CultureInfo.CurrentCulture);
    }

    private async Task CalculateAsync(Func<double, double, double> operation)
    {
        if (!TryGetInputs(out double left, out double right))
        {
            await ShowAlertAsync(
                "קלט לא תקין",
                "אנא הכנס מספרים תקינים בשתי התיבות",
                "אישור");

            return;
        }

        Result = operation(left, right)
            .ToString(CultureInfo.CurrentCulture);
    }

    private bool TryGetInputs(out double left, out double right)
    {
        bool isLeftValid = double.TryParse(Number1, out left);
        bool isRightValid = double.TryParse(Number2, out right);

        return isLeftValid && isRightValid;
    }
}
