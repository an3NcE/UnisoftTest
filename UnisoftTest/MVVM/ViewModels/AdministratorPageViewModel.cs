using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnisoftTest;
using UnisoftTest.MVVM.Models;
using UniToolbox.MVVM.Models;

namespace UniToolbox.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class AdministratorPageViewModel
    {
        public AppSettings AppSettingsPassword { get; set; }
        string adminPassword = "adminPassword";
        public Modules CurrentModule { get; set; }


        public List<Modules> AllModules { get; set; }

        public ICommand SaveNewPassworCm => new Command(SaveNewPassword);
        public ICommand DeleteModules => new Command(DeleteAllModules);
        public ICommand VisualStatePage => new Command(AddModuleToView);
        public AdministratorPageViewModel()
        {
            CurrentModule = new Modules();
            Refresh();

            AllModules = App.BaseRepo.GetAllModules();
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

        //public void ChangeVisualState()
        //{
        //    if (CurrentModule.ModuleAccess == true)
        //    {
        //        CurrentModule.ModuleAccess=false;
        //    }
        //    else
        //    {
        //        CurrentModule.ModuleAccess=true;
        //    }
        //    App.BaseRepo.AddOrUpdateModule(CurrentModule);
        //    Refresh();
        //}

        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;

                }
            }
        }
        public void AddModuleToView(object obj)
        {
            var currentScript = obj as Modules;
            if (_isChecked == true || (!currentScript.ModuleAccess))
            {
                currentScript.ModuleAccess = true;
                currentScript.ImgVisualState = "unfav.png";
            }
            else
            {
                currentScript.ModuleAccess = false;
                currentScript.ImgVisualState = "fav.png";
            }
            App.BaseRepo.VisualModuleSTatus(currentScript);
            Refresh();
        }

        private void Refresh()
        {
            AllModules = App.BaseRepo.GetAllModules();

            AppSettingsPassword = App.BaseRepo.GetSettings(2);
            if (AppSettingsPassword == null)
            {
                SaveNewPassword();
            }
            //AppSettingsExePath = new AppSettings();

        }
        private void DeleteAllModules()
        {
            App.BaseRepo.DeleteAllModules();
            Refresh();
        }

        
    }
}
