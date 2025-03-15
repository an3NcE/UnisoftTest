using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnisoftTest.MVVM.Models
{
    [Table("AppSettings")]
    public class AppSettings
    {

        [PrimaryKey]
        public int SettingsId { get; set; }
        public string SettingsName { get; set; }
        public string SettingsValue { get; set; }

        public DateTime SettingsCreatedAt { get; set; }

    }


}
// exe path script id 0
// administrator  status id 1
// pw administrator id 2   
// widocznosc modyfikatorow customscript id 3   