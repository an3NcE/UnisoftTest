using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnisoftTest.MVVM.Models
{
    [Table("BackupServiceConfiguration")]
    public class BackupServiceConfiguration
    {

        [PrimaryKey, AutoIncrement]
        public int backupserviceconfId { get; set; }
        public string backupserviceconf_smtpserver { get; set; }
        public string backupserviceconf_smtpport { get; set; }
        public string backupserviceconf_smtpclientaddresss { get; set; }
        public string backupserviceconf_smtpclientpassword { get; set; }
        public string backupserviceconf_emailreceiver { get; set; }
        

        public DateTime CreateConfDate { get; set; }
        


    }
}
