using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblProdStock_dtl", Schema = "dbo")]
    public partial class TblProdStockDtl :TblBase, IEntity
    {
        [Key]
        public long PkstockId { get; set; }
        public long FKProdID { get; set; }
        public long FKLocationID { get; set; }//=0
        public long FKLotID { get; set; }
         public decimal OpStock { get; set; }
        public decimal InStock { get; set; }
        public decimal OutStock { get; set; }
        public decimal CurStock { get; set; }

        //public TblProdLotDtl Fk { get; set; }
        //[ForeignKey("FKLocationID")]
        //public TblLocMas Fklocation { get; set; }
        //public TblProdMas Fkprod { get; set; }
    }
}
