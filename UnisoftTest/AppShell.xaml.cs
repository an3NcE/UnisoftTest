using UnisoftTest.MVVM.Views;

namespace UnisoftTest
{
    public partial class AppShell : Shell
    {
        public bool isAdministrator { get; set; }
        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;
            //Routing.RegisterRoute("ResultPageHome", typeof(ResultPage)); // Rejestracja trasy dla ResultPage
            //Routing.RegisterRoute("ConfigurationPageRoute", typeof(ConfigurationPage)); // Rejestracja trasy dla ConfigurationPage
            App.BaseRepo.AddOrUpdateAppAdministrator(true);
            isAdministrator = false;
        }

        private void SetAdministrator(object sender, EventArgs e)
        {
            if (isAdministrator == false)
            {
                isAdministrator = true;
                lblAdministrator.Text = "Jesteś Administratorem! :)";
            }
            else
            {
                isAdministrator = false;
                lblAdministrator.Text = "Czy jesteś Administratorem?";
            }
            
        }
    }
}
