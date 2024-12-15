using UnisoftTest.MVVM.Views;
using UnisoftTest.Repositories;

namespace UnisoftTest
{
    public partial class App : Application
    {
        public static BaseRepository BaseRepo { get; private set; }

        public App(BaseRepository repo)
        {
            InitializeComponent();
            BaseRepo = repo;

            MainPage = new NavigationPage( new ResultPage());

            
        }
    }
}
