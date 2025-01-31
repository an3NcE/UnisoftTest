using UnisoftTest.MVVM.Views;

namespace UnisoftTest
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute("ResultPageHome", typeof(ResultPage)); // Rejestracja trasy dla ResultPage
            //Routing.RegisterRoute("ConfigurationPageRoute", typeof(ConfigurationPage)); // Rejestracja trasy dla ConfigurationPage
            App.BaseRepo.AddOrUpdateAppAdministrator(true);
            Routing.RegisterRoute("ResultPage", typeof(ResultPage));
        }

       
    }
}
