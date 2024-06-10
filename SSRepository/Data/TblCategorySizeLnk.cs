using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("TblCategorySize_lnk", Schema = "dbo")]
    public partial class TblCategorySizeLnk : TblBase, IEntity
    {
        [Key]
        public long PkId { get; set; }
        public long FkCategoryId { get; set; }
        public string Size { get; set; }
    }
}
