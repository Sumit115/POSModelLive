using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblProdQTYBarcode", Schema = "dbo")]
    public partial class  TblProductQTYBarcode
    {
        [Key] 
        public long PkId { get; set; }
        public long FkLotID { get; set; }
        public string Barcode { get; set; }
        public Nullable<long> FKLocationId { get; set; }
        public Nullable<long> TranInId { get; set; }
        public Nullable<long> TranInSeriesId { get; set; }
        public Nullable<long> TranInSrNo { get; set; }
        public Nullable<long> TranOutId { get; set; }
        public Nullable<long> TranOutSeriesId { get; set; }
        public Nullable<long> TranOutSrNo { get; set; }
    }
}
