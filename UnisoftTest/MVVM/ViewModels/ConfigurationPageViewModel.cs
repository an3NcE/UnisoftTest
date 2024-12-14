
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

        public AutoItScript CurrentScript { get; set; }

        public ICommand AddOrUpdateCommand => new Command(AddOrUpdateComm);
        public ICommand DeleteCommand => new Command(DeleteComm);

       

        //#region EntryInf
        //private string _nameScript;
        //private string _pathScript;
        //#endregion


        public ConfigurationPageViewModel()
        {
            CurrentScript = new AutoItScript();


            Refresh();



        }

        private void DeleteComm(object obj)
        {
            App.BaseRepo.Delete(CurrentScript.ScriptId);
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
                    App.BaseRepo.AddOrUpdate(CurrentScript);

                    Debug.WriteLine(App.BaseRepo.StatusMessage);
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



            Refresh();
        }
      


        private void Refresh()
        {
            
            Scripts = App.BaseRepo.GetAll();
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
