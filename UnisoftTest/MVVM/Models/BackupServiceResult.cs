using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unisofttest.MVVM.Models
{
    [Table("BackupServiceResult")]
    public class BackupServiceResult
    {
        [PrimaryKey, AutoIncrement]
        public int backupserviceresult_Id { get; set; }
        public string backupserviceresult_clientname { get; set; }
        public string backupserviceresult_clientsymbol { get; set; }
        public string backupserviceresult_result { get; set; }
        public string backupserviceresult_resultimage { get; set; }
        public byte[] backupserviceresult_resultlog { get; set; }
        public DateTime backupserviceresult_resultlogDate { get; set; }


    }
}
