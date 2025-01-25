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
        public CopyBaseScripts CurrentScript { get; set; }

        public ICommand AddOrUpdateCommand => new Command(AddOrUpdateComm);

        public CopyBaseConfigurationPageViewModel()
        {
            CurrentScript = new CopyBaseScripts();
        }

        private async void AddOrUpdateComm()
        {


            if (CurrentScript == null)
            {
                CurrentScript = new CopyBaseScripts();
            }

            if (!string.IsNullOrEmpty(CurrentScript.SourceBaseName) || !string.IsNullOrEmpty(CurrentScript.DestinationBaseName) || !string.IsNullOrEmpty(CurrentScript.CopyBaseScript))
            {
                App.BaseRepo.AddOrUpdateBaseScript(CurrentScript);

                Debug.WriteLine(App.BaseRepo.StatusMessage);
                Refresh();


            }
            else
            {
                MessagingCenter.Send(this, "Alert", "Wypełnij wszystkie pola.");
            }




        }
    }
}
