using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnisoftTest.MVVM.Models
{
    [Table("CustomScripts")]
    public class CustomScripts
    {
        [PrimaryKey, AutoIncrement]
        public int CustomScriptId { get; set; }
        public string CustomScriptName { get; set; }
        
        public string CustomScriptCMD { get; set; }
        public string CustomScriptSQL { get; set; }

        public DateTime CreateScriptDate { get; set; }
        
    }
}
