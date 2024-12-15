using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnisoftTest.MVVM.Models;
using UnisoftTest.MVVM.Views;

namespace UnisoftTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ResultPageViewModel
    {
        public List<AutoItScript> FavScripts { get; set; }
        public AutoItScript CurrentFAVScript { get; set; }

        public ICommand GoToConfPage => new Command(GoToConfigurationPage);
        public ICommand RunScript => new Command(RunTestScript);

        

        public ResultPageViewModel()
        {
            CurrentFAVScript = new AutoItScript();
            Refresh();
        }

        public void Refresh()
        {
            FavScripts = App.BaseRepo.GetAllFav();
        }
        private void RunTestScript(object obj)
        {
            var favtest = obj;
        }

        private void GoToConfigurationPage(object obj)
        {
            (Application.Current.MainPage as NavigationPage)?.PushAsync(new ConfigurationPage());
        }
    }
}
