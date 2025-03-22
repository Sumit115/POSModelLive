using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblAccountGroup_mas", Schema = "dbo")]
    public partial class TblAccountGroupMas : TblBase, IEntity
    {
        [Key] 
        public long PkAccountGroupId { get; set; }
        public long? FkAccountGroupId { get; set; }
        public string? GroupType { get; set; } 
        public string AccountGroupName { get; set; }
        public string? GroupAlias { get; set; }
        public string? NatureOfGroup { get; set; }
        public bool PrintDtl { get; set; }
        public bool NetCrDrBalanceForRpt { get; set; }
         
        public virtual TblAccountGroupMas? FKAccountGroupMas { get; set; }
    }
}
