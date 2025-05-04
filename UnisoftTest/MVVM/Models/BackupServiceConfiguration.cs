using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnisoftTest.Helpers;

namespace UnisoftTest.MVVM.Models
{
    [Table("BackupServiceConfiguration")]
    public class BackupServiceConfiguration
    {

        [PrimaryKey, AutoIncrement]
        public int backupserviceconf_Id { get; set; }
        public string backupserviceconf_loginserver { get; set; }
        public string backupserviceconf_passwordserver { get; set; }
        public string backupserviceconf_instance { get; set; }
        public string backupserviceconf_directory { get; set; }
        public string backupserviceconf_dumpfile{ get; set; }
        public string backupserviceconf_logfile { get; set; }
        public string backupserviceconf_schemas { get; set; }
        public string backupserviceconf_mailreceiver { get; set; }
        public string backupserviceconf_mailtitle { get; set; }
        public string backupserviceconf_addedCmdScript { get; set; }
        public int backupserviceconf_scheduletime_hour { get; set; }
        public int backupserviceconf_scheduletime_minutes { get; set; }
        public int backupserviceconf_daysofweek { get; set; } //0=Pn-Pt 1=Pn-Nd

        [Ignore]
        public string backupserviceconf_passwordserverDecryptedPassword
        {
            get => AesEncryptionHelper.Decrypt(backupserviceconf_passwordserver);
            set => backupserviceconf_passwordserver = AesEncryptionHelper.Encrypt(value);
        }


        public DateTime backupserviceconf_CreateConfDate { get; set; }
        


    }
}
