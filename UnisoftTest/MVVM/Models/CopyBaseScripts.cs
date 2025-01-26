using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTest.MVVM.Models
{
    [Table("CopyBaseScripts")]
    public class CopyBaseScripts
    {
        [PrimaryKey, AutoIncrement]
        public int BaseScriptId { get; set; }
        public string SourceBaseName { get; set; }
        public string DestinationBaseName { get; set; }
        public string CopyBaseScript { get; set; }

        public DateTime CreateScriptDate { get; set; }
        public string DisplayName => $"{SourceBaseName} -> {DestinationBaseName}";

    }
}
