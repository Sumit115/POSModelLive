using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblPromotion_mas", Schema = "dbo")]
    public partial class TblPromotionMas : TblBase, IEntity
    {


        [Key]
        public long PkPromotionId { get; set; }
        public string PromotionDuring { get; set; }//S=Sale,P=Purchase 
        public string PromotionName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PromotionFromDt { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PromotionToDt { get; set; }
        public string? PromotionFromTime { get; set; }
        public string? PromotionToTime { get; set; }
        public long? FKLocationId { get; set; }
        public long? FkVendorId { get; set; }
        public long? FkCustomerId { get; set; }
        public long? FkReferById { get; set; }
        public string PromotionApplyOn { get; set; }
        public string Promotion { get; set; }
        public decimal? PromotionApplyAmt { get; set; }
        public decimal? PromotionApplyAmt2 { get; set; }
        public decimal? PromotionApplyQty { get; set; }
        public decimal? PromotionApplyQty2 { get; set; }
        public long? FkPromotionApplyUnitId { get; set; }
        public long? FKLotID { get; set; }//Hide FOr Nor

        public long? FkPromotionProdId { get; set; }
        public decimal? PromotionAmt { get; set; }
        public decimal? PromotionQty { get; set; }
        public long? FkPromotionUnitId { get; set; }


        public long? FKProdID { get; set; }
        public long? FkProdCatgId { get; set; }
        public long? FkBrandId { get; set; }
        public long SequenceNo { get; set; }

        public virtual TblUserMas FKUser { get; set; }
          public virtual TblBrandMas? FkBrand { get; set; } 
    }
}
