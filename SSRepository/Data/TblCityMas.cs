using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblCity_mas", Schema = "dbo")]
    public partial class TblCityMas : TblBase,IEntity
    {
       [Key]
        public long PkCityId { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }

    }
}
