using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using unisofttest.MVVM.Models;
using UnisoftTest;

namespace unisofttest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class BackupServiceResultPageViewModel
    {
        public List<BackupServiceResult> Clients { get; set; }
        public BackupServiceResult CurrentClient { get; set; }

        public ICommand AddOrUpdateCommand => new Command(AddOrUpdateComm);

        public BackupServiceResultPageViewModel()
        {
            //CurrentScript = new CopyBaseScripts();
            Refresh();
        }

        private async void AddOrUpdateComm()
        {


            if (CurrentClient == null)
            {
                CurrentClient = new BackupServiceResult();
            }

            //if (!string.IsNullOrEmpty(CurrentClient.SourceBaseName) & !string.IsNullOrEmpty(CurrentScript.DestinationBaseName) & !string.IsNullOrEmpty(CurrentScript.CopyBaseScript))
            //{
            await App.BaseRepo.AddOrUpdateBackupServiceResult(CurrentClient);

            Debug.WriteLine(App.BaseRepo.StatusMessage);
              await Refresh();


            //}
            //else
            //{
            //    MessagingCenter.Send(this, "Alert", "Wypełnij wszystkie pola.");
            //}

        }

        private async Task Refresh()
        {
            Clients = await App.BaseRepo.GetAllBackupServiceResults();
            CurrentClient = new BackupServiceResult();
        }

    }
}
