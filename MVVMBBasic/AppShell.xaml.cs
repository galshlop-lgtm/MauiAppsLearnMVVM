using MVVMBBasic.Views;

namespace MVVMBBasic
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("CalcAge", typeof(CalcAgePage));
            Routing.RegisterRoute("CalculatorPage", typeof(CalculatorPage));
            Routing.RegisterRoute("ChangeCalculator", typeof(ChangeCalculatorPage));
        }
    }
}
