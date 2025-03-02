using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnisoftTest.MVVM.Models;
using System.Diagnostics;

namespace UnisoftTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CustomScriptsPageViewModel
    {
        public bool isAdministrator { get; set; }
        public AppSettings AppSettingsAdministrator { get; set; }
        public List<CustomScripts> CustomScripts { get; set; }
        public CustomScripts CurrentScript { get; set; }

        public ICommand AddOrUpdateCommand => new Command(AddOrUpdateComm);
        public ICommand DeleteCommand => new Command(DeleteComm);
        public CustomScriptsPageViewModel()
        {
            Refresh();
        }
        public void Refresh()
        {

            CustomScripts = App.BaseRepo.GetAllCustomScripts();
            CurrentScript = new CustomScripts();

            AppSettingsAdministrator = App.BaseRepo.GetAdministratorStatus();
            
            if (AppSettingsAdministrator.SettingsValue == "0")
            {
                isAdministrator = false;
            }
            else
            {
                isAdministrator = true;
            }
            //AppSettingsExePath = new AppSettings();

        }

        private async void AddOrUpdateComm()
        {


            if (CurrentScript == null)
            {
                CurrentScript = new CustomScripts();
            }

            if (!string.IsNullOrEmpty(CurrentScript.CustomScriptName)  & !string.IsNullOrEmpty(CurrentScript.CustomScriptSQL))
            {
                App.BaseRepo.AddOrUpdateCustomScript(CurrentScript);

                Debug.WriteLine(App.BaseRepo.StatusMessage);
                Refresh();


            }
            else
            {
                MessagingCenter.Send(this, "Alert", "Wypełnij wszystkie pola.");
            }

        }

        private void DeleteComm(object obj)
        {
            App.BaseRepo.DeleteCustomScript(CurrentScript.CustomScriptId);
            Refresh();
        }
    }
}
