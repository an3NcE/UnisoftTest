using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unisofttest.MVVM.Models
{
    [Table("MailConfiguration")]
    public class MailConfiguration
    {

        [PrimaryKey, AutoIncrement]
        public int mailconf_id { get; set; }
        public string mailconf_smtpserver { get; set; }
        public string mailconf_smtpport { get; set; }
        public string mailconf_smtpclientaddresss { get; set; }
        public string mailconf_smtpclientpassword { get; set; }
        public string mailconf_emailreceiver { get; set; }


        public DateTime mailconf_createdate { get; set; }

    }
}
