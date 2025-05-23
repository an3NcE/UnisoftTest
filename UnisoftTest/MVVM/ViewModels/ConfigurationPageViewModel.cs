﻿
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnisoftTest.MVVM.Models;

namespace UnisoftTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ConfigurationPageViewModel
    {

        public List<AutoItScript> Scripts { get; set; }
        public List<AppSettings> AppSettingsList { get; set; }

        public AutoItScript CurrentScript { get; set; }
        public AppSettings AppSettingsExePath { get; set; }

        string pathAutoItexeName = "AutoItExePath"; 

        public ICommand AddOrUpdateCommand => new Command(AddOrUpdateComm);
        public ICommand DeleteCommand => new Command(DeleteComm);
        public ICommand AddToFavorite => new Command(AddToFav);
        public ICommand SavePathAutoItExe => new Command(SavePathExe);

        






        //#region EntryInf
        //private string _nameScript;
        //private string _pathScript;
        //#endregion


        public ConfigurationPageViewModel()
        {
            CurrentScript = new AutoItScript();
            //AppSettingsExePath = new AppSettings();
            //App.BaseRepo.DeleteSet();


            Refresh();



        }

        private async void SavePathExe()
        {
            if (AppSettingsExePath == null)
            {
                AppSettingsExePath = new AppSettings();
            }
            AppSettingsExePath.SettingsId = 0;
            AppSettingsExePath.SettingsName = pathAutoItexeName;
            //if (AppSettingsExePath.SettingsValue != null)
            //{
            //    AppSettingsExePath.SettingsValue = AppSettingsExePath.SettingsValue.Replace("\"", "");
            //}
            

            await App.BaseRepo.AddOrUpdateAppSettingsPathExe(AppSettingsExePath);
            Refresh();
        }

        private async void DeleteComm(object obj)
        {
            await App.BaseRepo.Delete(CurrentScript.ScriptId);
            Refresh();
        }


        private async void AddOrUpdateComm()
        {
            

            if (CurrentScript == null)
            {
                CurrentScript = new AutoItScript();
            }

            if (!string.IsNullOrEmpty(CurrentScript.ScriptName) || !string.IsNullOrEmpty(CurrentScript.ScriptPath))
            {
                CurrentScript.ScriptPath = CurrentScript.ScriptPath.Replace("\"", "");
                if (File.Exists(CurrentScript.ScriptPath))
                {
                    //CurrentScript.ScriptName = _nameScript;
                    //CurrentScript.ScriptPath = _pathScript;
                    await App.BaseRepo.AddOrUpdate(CurrentScript);
                    

                    Debug.WriteLine(App.BaseRepo.StatusMessage);
                    Refresh();
                }
                else
                {
                    MessagingCenter.Send(this, "Alert", "Ścieżka skryptu jest błędna.");
                }

            }
            else
            
            
            
            {
                MessagingCenter.Send(this, "Alert", "Nazwa lub ścieżka skryptu jest pusta.");
            }



            
        }
      


        private async void Refresh()
        {
            //AppSettingsList = App.BaseRepo.GetAllSettings();
            Scripts = await App.BaseRepo.GetAll();
            CurrentScript = new AutoItScript();
            IsChecked = false;
            //AppSettingsExePath = new AppSettings();

            AppSettingsExePath = await App.BaseRepo.GetSettings(0);
            if (AppSettingsExePath == null)
            {
                SavePathExe();
            }
            //AppSettingsExePath = new AppSettings();

        }

        
        
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

        public void AddToFav(object obj)
        {
            var currentScript = obj as AutoItScript;
            if (_isChecked == true || (!currentScript.IsFavorite ))
            {
                currentScript.IsFavorite = true;
                currentScript.ImgFav = "unfav.png";
            }else
            {
                currentScript.IsFavorite = false;
                currentScript.ImgFav = "fav.png";
            }
            App.BaseRepo.FavScript(currentScript);
            Refresh();
        }


        //#region EntryValues



        //public string NameScriptEntry
        //{
        //    get => _nameScript;
        //    set
        //    {

        //        _nameScript = value;

        //    }
        //}
        //public string PathScriptEntry
        //{
        //    get => _pathScript;
        //    set
        //    {

        //        _pathScript = value;

        //    }
        //}
        //#endregion

    }
}
