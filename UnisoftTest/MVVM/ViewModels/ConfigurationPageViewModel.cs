using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnisoftTest.MVVM.Models;

namespace UnisoftTest.MVVM.ViewModels
{
    public class ConfigurationPageViewModel
    {
        public List<AutoItScript> Scripts { get; set; }

        public AutoItScript CurrentScript { get; set; }
    }
}
