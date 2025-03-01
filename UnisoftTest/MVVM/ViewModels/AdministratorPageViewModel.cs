using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnisoftTest;
using UnisoftTest.MVVM.Models;

namespace UniToolbox.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class AdministratorPageViewModel
    {
        public AppSettings AppSettingsPassword { get; set; }
        string adminPassword = "adminPassword";

        public ICommand SaveNewPassworCm => new Command(SaveNewPassword);
        public AdministratorPageViewModel()
        {
            Refresh();
        }

        private void SaveNewPassword()
        {
            if (AppSettingsPassword == null)
            {
                AppSettingsPassword = new AppSettings();
            }
            AppSettingsPassword.SettingsId = 2;
            AppSettingsPassword.SettingsName = adminPassword;
            if (AppSettingsPassword.SettingsValue != null)
            {
                AppSettingsPassword.SettingsValue = AppSettingsPassword.SettingsValue.Replace("\"", "");
            }


            App.BaseRepo.AddOrUpdateAppSettingsPathExe(AppSettingsPassword);
            Refresh();
        }

        private void Refresh()
        {


            AppSettingsPassword = App.BaseRepo.GetSettings(2);
            if (AppSettingsPassword == null)
            {
                SaveNewPassword();
            }
            //AppSettingsExePath = new AppSettings();

        }
    }
}
