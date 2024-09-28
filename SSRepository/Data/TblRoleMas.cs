using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblRole_mas", Schema = "dbo")]
    public partial class TblRoleMas : TblBase, IEntity
    {
        [Key]
        public long PkRoleId { get; set; }
        public string RoleName { get; set; }
        

    }
}
