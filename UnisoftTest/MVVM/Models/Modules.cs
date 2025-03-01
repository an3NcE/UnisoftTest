using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniToolbox.MVVM.Models
{
    [Table("Modules")]
    public class Modules
    {
        [PrimaryKey]
        public int ModulID { get; set; }
        public string ModuleName { get; set; }
        public bool ModuleAccess { get; set; }

        public DateTime LastModified { get; set; }
    }
}
