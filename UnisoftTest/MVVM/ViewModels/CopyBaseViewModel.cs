using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTest.MVVM.Models;

namespace UnisoftTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CopyBasePageViewModel
    {
        public List<CopyBaseScripts> BaseScripts { get; set; }

        public CopyBasePageViewModel()
        {
            Refresh();
        }


        public void Refresh()
        {

            BaseScripts = App.BaseRepo.GetAllBaseScripts();
            //CurrentScript = new CopyBaseScripts();


            //AppSettingsExePath = new AppSettings();

        }

    }
}
