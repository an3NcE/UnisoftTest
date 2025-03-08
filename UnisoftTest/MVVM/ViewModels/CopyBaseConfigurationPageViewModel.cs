using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnisoftTest;
using UniTest.MVVM.Models;

namespace UniTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CopyBaseConfigurationPageViewModel
    {
        public List<CopyBaseScripts> BaseScripts { get; set; }
        public CopyBaseScripts CurrentScript { get; set; }

        public ICommand AddOrUpdateCommand => new Command(AddOrUpdateComm);
        public ICommand DeleteCommand => new Command(DeleteComm);

        public CopyBaseConfigurationPageViewModel()
        {
            //CurrentScript = new CopyBaseScripts();
            Refresh();
        }

        private async void AddOrUpdateComm()
        {


            if (CurrentScript == null)
            {
                CurrentScript = new CopyBaseScripts();
            }

            if (!string.IsNullOrEmpty(CurrentScript.SourceBaseName) & !string.IsNullOrEmpty(CurrentScript.DestinationBaseName) & !string.IsNullOrEmpty(CurrentScript.CopyBaseScript))
            {
                await App.BaseRepo.AddOrUpdateBaseScript(CurrentScript);

                Debug.WriteLine(App.BaseRepo.StatusMessage);
                await Refresh();


            }
            else
            {
                MessagingCenter.Send(this, "Alert", "Wypełnij wszystkie pola.");
            }

        }
        private async Task Refresh()
        {

            BaseScripts = await App.BaseRepo.GetAllBaseScripts();
            CurrentScript = new CopyBaseScripts();
            

            //AppSettingsExePath = new AppSettings();

        }

        private async void DeleteComm(object obj)
        {
            await App.BaseRepo.DeleteBaseScript(CurrentScript.BaseScriptId);
            Refresh();
        }
    }
}
