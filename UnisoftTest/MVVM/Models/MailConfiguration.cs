using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnisoftTest.Helpers;

namespace unisofttest.MVVM.Models
{
    [Table("MailConfiguration")]
    public class MailConfiguration
    {

        [PrimaryKey, AutoIncrement]
        public int mailconf_id { get; set; }
        public string mailconf_smtpserver { get; set; }
        public int mailconf_smtpport { get; set; }
        public string mailconf_smtpclientaddresss { get; set; }
        public string mailconf_smtpclientpassword { get; set; }

        [Ignore]
        public string DecryptedPassword
        {
            get => AesEncryptionHelper.Decrypt(mailconf_smtpclientpassword);
            set => mailconf_smtpclientpassword = AesEncryptionHelper.Encrypt(value);
        }

        public DateTime mailconf_createdate { get; set; }

    }
}
