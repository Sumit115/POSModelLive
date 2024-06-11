using System.ComponentModel.DataAnnotations;

namespace SSRepository.Models
{
 
    public class  SysDefaultsModel
    {

        [Key]
        public long PKSysDefID { get; set; }
        public string SysDefKey { get; set; }
        public string SysDefValue { get; set; }
        public string FKTableName { get; set; }
        public string FKColumnName { get; set; }
        public long FKUserID { get; set; }
        public System.DateTime DATE_MODIFIED { get; set; }

    }
}
