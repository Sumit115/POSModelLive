using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Data
{
    [Table("tblSysDefaults", Schema = "dbo")]
    public class TblSysDefaults
    {
        public TblSysDefaults() { }
        [Key]
        public long PKSysDefID {  get; set; }   
        public string SysDefKey { get; set; }
        public string SysDefValue { get; set;}
        public string? FKTableName { get; set;}
        public string? FKColumnName { get; set;}
        public long FKUserID { get; set;}
    }
}
