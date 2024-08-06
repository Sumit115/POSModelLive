using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class PromotionModel : BaseModel
    {

        public long PkPromotionId { get; set; }
        public string PromotionDuring { get; set; }//S=Sales,P=Purchase 

        [Required(ErrorMessage = "Name Required")]
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
       
        [Required(ErrorMessage = "Apply On Required")]
        public string PromotionApplyOn { get; set; }
        [Required(ErrorMessage = "Promotion Required")]
        public string  Promotion { get; set; }
        public decimal? PromotionApplyAmt { get; set; }
        public decimal? PromotionApplyQty { get; set; }
        public long? FkPromotionApplyUnitId { get; set; }
        public long? FKLotID { get; set; }//Hide FOr Nor

        public long? FkPromotionProdId { get; set; }
        public decimal? PromotionAmt { get; set; }
        public decimal? PromotionQty { get; set; }
        public string? FkPromotionUnitId { get; set; }


        public long? FKProdID { get; set; }
        public long? FkProdCatgId { get; set; }
        public long? FkBrandId { get; set; }

        public string? CustomerName { get; set; }
        public string? VendorName { get; set; }
        public string? LocationName { get; set; }

    }

}
