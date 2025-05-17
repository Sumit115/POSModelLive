using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblDistrict_mas", Schema = "dbo")]
    public partial class TblDistrictMas : TblBase, IEntity
    {
        [Key]
        public long PkDistrictId { get; set; }
         
        public string DistrictName { get; set; }
        public long FkStateId { get; set; }
        public virtual TblStateMas FKState { get; set; }

    }
}
