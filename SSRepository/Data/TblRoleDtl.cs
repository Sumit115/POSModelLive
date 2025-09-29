using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblRole_Dtl", Schema = "dbo")]
    public partial class TblRoleDtl : IEntity
    {
        [Key]
        public long PkRoleDtlId { get; set; }
        public long FKFormID { get; set; }
        public bool IsAccess { get; set; }
        public bool IsEdit { get; set; }
        public bool IsCreate { get; set; }
        public bool IsPrint { get; set; }
        public bool IsBrowse { get; set; }
        public bool IsDelete { get; set; }
        public long FkRoleID { get; set; }

    }
}
