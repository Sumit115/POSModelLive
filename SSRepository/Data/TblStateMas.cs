using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblState_mas", Schema = "dbo")]
    public partial class TblStateMas : TblBase, IEntity
    {
        [Key]
        public long PkStateId { get; set; }
        public string StateName { get; set; }
        public long FkCountryId { get; set; }
        public string? CapitalName { get; set; }
        public string? StateType { get; set; }
        public string? StateCode { get; set; }
    }
}
