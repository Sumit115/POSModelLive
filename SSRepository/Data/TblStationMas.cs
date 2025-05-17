using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblStation_mas", Schema = "dbo")]
    public partial class TblStationMas : TblBase, IEntity
    {
        [Key]
        public long PkStationId { get; set; }
         
        public string StationName { get; set; }
        public long FkDistrictId { get; set; }
        public virtual TblDistrictMas FKDistrict { get; set; }

    }
}
