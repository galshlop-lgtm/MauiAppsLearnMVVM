using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MVVMBBasic.ViewModels;

public sealed class CalculatorViewModel : INotifyPropertyChanged
{
    private string _display = "0";
    private decimal _storedValue;
    private decimal _lastOperand;
    private string? _pendingOperator;
    private string? _lastOperator;
    private bool _isNewInput = true;
    private bool _hasError;

    public CalculatorViewModel()
    {
        NumberCommand = new Command<string>(AppendNumber);
        DecimalCommand = new Command(AddDecimal);
        OperatorCommand = new Command<string>(SetOperator);
        EqualsCommand = new Command(CalculateResult);
        ClearCommand = new Command(Clear);
        DeleteCommand = new Command(DeleteLastCharacter);
        ToggleSignCommand = new Command(ToggleSign);
        PercentCommand = new Command(ConvertToPercent);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Display
    {
        get => _display;
        private set
        {
            if (_display == value)
            {
                return;
            }

            _display = value;
            OnPropertyChanged();
        }
    }

    public ICommand NumberCommand { get; }
    public ICommand DecimalCommand { get; }
    public ICommand OperatorCommand { get; }
    public ICommand EqualsCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ToggleSignCommand { get; }
    public ICommand PercentCommand { get; }

    private void AppendNumber(string? number)
    {
        if (_hasError)
        {
            Clear();
        }

        if (string.IsNullOrEmpty(number) ||
            number.Length != 1 ||
            !char.IsDigit(number[0]))
        {
            return;
        }

        if (_isNewInput)
        {
            Display = number;
            _isNewInput = false;
            return;
        }

        if (Display == "0")
        {
            Display = number;
        }
        else if (Display == "-0")
        {
            Display = $"-{number}";
        }
        else
        {
            Display += number;
        }
    }

    private void AddDecimal()
    {
        if (_hasError)
        {
            Clear();
        }

        if (_isNewInput)
        {
            Display = "0.";
            _isNewInput = false;
            return;
        }

        if (!Display.Contains('.', StringComparison.Ordinal))
        {
            Display += ".";
        }
    }

    private void SetOperator(string? operatorSymbol)
    {
        if (_hasError)
        {
            Clear();
        }

        var operation = NormalizeOperator(operatorSymbol);

        if (operation is null || !TryGetDisplayValue(out var currentValue))
        {
            return;
        }

        try
        {
            if (_pendingOperator is not null && !_isNewInput)
            {
                _storedValue = ApplyOperation(
                    _storedValue,
                    currentValue,
                    _pendingOperator);

                Display = FormatValue(_storedValue);
            }
            else if (_pendingOperator is null)
            {
                _storedValue = currentValue;
            }

            _pendingOperator = operation;
            _lastOperator = null;
            _isNewInput = true;
        }
        catch (ArithmeticException)
        {
            ShowError();
        }
    }

    private void CalculateResult()
    {
        if (_hasError)
        {
            return;
        }

        try
        {
            if (_pendingOperator is not null)
            {
                if (!TryGetDisplayValue(out var rightOperand))
                {
                    ShowError();
                    return;
                }

                _lastOperand = rightOperand;
                _storedValue = ApplyOperation(
                    _storedValue,
                    rightOperand,
                    _pendingOperator);

                _lastOperator = _pendingOperator;
                _pendingOperator = null;
                Display = FormatValue(_storedValue);
                _isNewInput = true;
                return;
            }

            if (_lastOperator is not null &&
                TryGetDisplayValue(out var currentValue))
            {
                _storedValue = ApplyOperation(
                    currentValue,
                    _lastOperand,
                    _lastOperator);

                Display = FormatValue(_storedValue);
                _isNewInput = true;
            }
        }
        catch (ArithmeticException)
        {
            ShowError();
        }
    }

    private void Clear()
    {
        _display = "0";
        _storedValue = 0;
        _lastOperand = 0;
        _pendingOperator = null;
        _lastOperator = null;
        _isNewInput = true;
        _hasError = false;

        OnPropertyChanged(nameof(Display));
    }

    private void DeleteLastCharacter()
    {
        if (_hasError)
        {
            Clear();
            return;
        }

        if (_isNewInput)
        {
            Display = "0";
            return;
        }

        if (Display.Length <= 1 ||
            (Display.Length == 2 && Display[0] == '-'))
        {
            Display = "0";
            _isNewInput = true;
            return;
        }

        Display = Display[..^1];

        if (Display is "-" or "")
        {
            Display = "0";
            _isNewInput = true;
        }
    }

    private void ToggleSign()
    {
        if (_hasError)
        {
            Clear();
        }

        if (_isNewInput && _pendingOperator is not null)
        {
            Display = "-0";
            _isNewInput = false;
            return;
        }

        if (Display == "0")
        {
            return;
        }

        Display = Display.StartsWith("-", StringComparison.Ordinal)
            ? Display[1..]
            : $"-{Display}";

        _isNewInput = false;
    }

    private void ConvertToPercent()
    {
        if (_hasError)
        {
            Clear();
        }

        if (!TryGetDisplayValue(out var value))
        {
            ShowError();
            return;
        }

        Display = FormatValue(value / 100);
        _isNewInput = true;
    }

    private bool TryGetDisplayValue(out decimal value)
    {
        return decimal.TryParse(
            Display,
            NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out value);
    }

    private static decimal ApplyOperation(
        decimal left,
        decimal right,
        string operation)
    {
        return operation switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" when right != 0 => left / right,
            "/" => throw new DivideByZeroException(),
            _ => left
        };
    }

    private static string? NormalizeOperator(string? operatorSymbol)
    {
        return operatorSymbol?.Trim() switch
        {
            "+" => "+",
            "-" or "−" => "-",
            "*" or "×" => "*",
            "/" or "÷" => "/",
            _ => null
        };
    }

    private static string FormatValue(decimal value)
    {
        return value.ToString("G29", CultureInfo.InvariantCulture);
    }

    private void ShowError()
    {
        _display = "Error";
        _storedValue = 0;
        _lastOperand = 0;
        _pendingOperator = null;
        _lastOperator = null;
        _isNewInput = true;
        _hasError = true;

        OnPropertyChanged(nameof(Display));
    }

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}ו