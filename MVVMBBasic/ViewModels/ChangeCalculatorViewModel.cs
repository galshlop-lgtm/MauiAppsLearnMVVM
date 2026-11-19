using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MVVMBBasic.ViewModels;

public sealed class ChangeCalculatorViewModel : BaseViewModel
{
    private string _amount = string.Empty;
    private string _conversionResult = "0.00";
    private string _message = string.Empty;
    private CurrencyOption? _fromCurrency;
    private CurrencyOption? _toCurrency;

    public ChangeCalculatorViewModel()
    {
        Title = "Currency Converter";

        Currencies = new ObservableCollection<CurrencyOption>
        {
            new("USD", "US Dollar", "$", 1.00m),
            new("EUR", "Euro", "€", 0.92m),
            new("GBP", "British Pound", "£", 0.79m),
            new("ILS", "Israeli Shekel", "₪", 3.65m),
            new("JPY", "Japanese Yen", "¥", 150.00m)
        };

        FromCurrency = Currencies[0];
        ToCurrency = Currencies[1];

        ConvertCommand = new Command(Convert);
        ClearCommand = new Command(Clear);
    }

    public ObservableCollection<CurrencyOption> Currencies { get; }

    public CurrencyOption? FromCurrency
    {
        get => _fromCurrency;
        set => SetProperty(ref _fromCurrency, value);
    }

    public CurrencyOption? ToCurrency
    {
        get => _toCurrency;
        set => SetProperty(ref _toCurrency, value);
    }

    public string Amount
    {
        get => _amount;
        set => SetProperty(ref _amount, value);
    }

    public string ConversionResult
    {
        get => _conversionResult;
        private set => SetProperty(ref _conversionResult, value);
    }

    public string Message
    {
        get => _message;
        private set => SetProperty(ref _message, value);
    }

    public ICommand ConvertCommand { get; }
    public ICommand ClearCommand { get; }

    public void Convert()
    {
        Message = string.Empty;

        if (!TryParseAmount(Amount, out decimal amount) || amount < 0)
        {
            ConversionResult = "—";
            Message = "Enter a valid non-negative amount.";
            return;
        }

        if (FromCurrency is null || ToCurrency is null)
        {
            ConversionResult = "—";
            Message = "Choose both currencies.";
            return;
        }

        decimal amountInUsd = amount / FromCurrency.UnitsPerUsd;
        decimal convertedAmount = amountInUsd * ToCurrency.UnitsPerUsd;

        ConversionResult = $"{ToCurrency.Symbol}{convertedAmount:N2} {ToCurrency.Code}";
    }

    private void Clear()
    {
        Amount = string.Empty;
        ConversionResult = "0.00";
        Message = string.Empty;
    }

    private static bool TryParseAmount(string value, out decimal amount)
    {
        return decimal.TryParse(
                   value,
                   NumberStyles.Number,
                   CultureInfo.CurrentCulture,
                   out amount)
               ||
               decimal.TryParse(
                   value,
                   NumberStyles.Number,
                   CultureInfo.InvariantCulture,
                   out amount);
    }
}

public sealed class CurrencyOption
{
    public CurrencyOption(
        string code,
        string name,
        string symbol,
        decimal unitsPerUsd)
    {
        Code = code;
        Name = name;
        Symbol = symbol;
        UnitsPerUsd = unitsPerUsd;
    }

    public string Code { get; }
    public string Name { get; }
    public string Symbol { get; }
    public decimal UnitsPerUsd { get; }

    public string DisplayName => $"{Name} ({Code} {Symbol})";
}
