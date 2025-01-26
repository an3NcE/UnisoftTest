using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UniTest.MVVM.Models;
using Windows.Media.Capture;

namespace UnisoftTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CopyBasePageViewModel
    {
        public List<CopyBaseScripts> BaseScripts { get; set; }
        public CopyBaseScripts CurrentScript { get; set; }

        public ICommand RunScript => new Command(RunCopyBaseScript);

        public CopyBasePageViewModel()
        {
            Refresh();
        }


        public void Refresh()
        {
            BaseScripts = App.BaseRepo.GetAllBaseScripts();

        }
        private async void RunCopyBaseScript(object obj)
        {
            if (obj == null)
            {
                return;
            }

            var currentCopyBaseScript = obj as CopyBaseScripts;

            string txt = currentCopyBaseScript.CopyBaseScript;


        }
    }
}
