using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{

    [Table("tblRecipe_dtl", Schema = "dbo")]
    public partial class TblRecipeDtl
    {
        [Key]
        public long FkRecipeId { get; set; }
        public long SrNo { get; set; }
        public string TranType { get; set; }//=I=IN,O=OUT 
        public long FkProductId { get; set; }
        public string? Batch { get; set; }
        public string? Color { get; set; } //Only Purchase
        public decimal Qty { get; set; }//txt

    }
}