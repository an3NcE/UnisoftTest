using UnisoftTest.MVVM.Views;
using UnisoftTest.Repositories;

namespace UnisoftTest
{
    public partial class App : Application
    {
        public static BaseRepository BaseRepo { get; private set; }

        public App(BaseRepository repo)
        {
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
            InitializeComponent();
            BaseRepo = repo;
            //Task.Run(async () => await App.BaseRepo.InitializeDatabaseAsync());
            //InitializeAppAsync();
            //MainPage = new NavigationPage( new ResultPage());
            MainPage = new AppShell();
            //NavigationPage.SetHasNavigationBar(this, false);

        }

        private async void InitializeAppAsync()
        {
             

            await BaseRepo.InitializeDatabaseAsync(); // Teraz czekasz na inicjalizację bazy!
            MainPage = new AppShell();
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);
            const int newWidth = 1000;
            const int newHeight = 770;

            window.MaximumWidth = newWidth;
            window.MaximumHeight = newHeight;
            window.MinimumWidth = newWidth;
            window.MinimumHeight = newHeight;
            

            return window;


        }

    }
}
