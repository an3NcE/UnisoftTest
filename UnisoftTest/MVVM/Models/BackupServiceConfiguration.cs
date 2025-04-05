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
        public int backupserviceconf_Id { get; set; }
        public string backupserviceconf_loginserver { get; set; }
        public string backupserviceconf_passwordserver { get; set; }
        public string backupserviceconf_directory { get; set; }
        public string backupserviceconf_dumpfile{ get; set; }
        public string backupserviceconf_logfile { get; set; }
        public string backupserviceconf_schemas { get; set; }
        public string backupserviceconf_mailreceiver { get; set; }
        public string backupserviceconf_mailtitle { get; set; }
        public TimeOnly backupserviceconf_scheduletime { get; set; }
        public int backupserviceconf_daysofweek { get; set; }
        

        public DateTime backupserviceconf_CreateConfDate { get; set; }
        


    }
}
