using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{

    [Table("tblEWayDtl_Lnk", Schema = "dbo")]
    public partial class TblEWayDtlLnk
    {
        [Key]
        public long FKID { get; set; }
        public long FkSeriesId { get; set; }
        public string? EWayNo { get; set; }
        public DateTime? EWayDate { get; set; }
        public string? VehicleNo { get; set; }
        public string? TransDocNo { get; set; }
        public DateTime? TransDocDate { get; set; }
        public string? TransMode { get; set; }
        public string? SupplyType { get; set; }
        public decimal? Distance { get; set; }
        public string? VehicleType { get; set; }

    }
}