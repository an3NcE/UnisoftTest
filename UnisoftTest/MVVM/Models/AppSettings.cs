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
