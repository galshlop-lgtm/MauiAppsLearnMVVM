using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MVVMBBasic.ViewModels
{
    public class CalcAgeViewModel : BaseViewModel
    {
        private string _yearOfBirth = string.Empty;
        private string _result = "Enter your year of birth.";

        public CalcAgeViewModel()
        {
            Title = "Age Calculator";
            CalculateAgeCommand = new Command(CalculateAge);
        }

        public ICommand CalculateAgeCommand { get; }

        public string YearOfBirth
        {
            get => _yearOfBirth;
            set => SetProperty(ref _yearOfBirth, value);
        }

        public string Result
        {
            get => _result;
            private set => SetProperty(ref _result, value);
        }

        public void CalculateAge()
        {
            if (!int.TryParse(YearOfBirth, out int birthYear))
            {
                Result = "Please enter a valid birth year.";
                return;
            }

            int currentYear = DateTime.Today.Year;
            if (birthYear < 1 || birthYear > currentYear)
            {
                Result = $"Enter a year between 1 and {currentYear}.";
                return;
            }

            int age = currentYear - birthYear;
            Result = $"Your current age is {age}.";
        }
    }
}
