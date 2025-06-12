using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class PromotionModel : BaseModel
    {

        public long PKID { get; set; }
        public string PromotionDuring { get; set; }//S=Sales,P=Purchase 

        [Required(ErrorMessage = "Name Required")]
        public string PromotionName { get; set; }

        public DateTime? PromotionFromDt { get; set; }
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
        public string Promotion { get; set; }
        public decimal? PromotionApplyAmt { get; set; }
        public decimal? PromotionApplyAmt2 { get; set; }
        public decimal? PromotionApplyQty { get; set; }
        public decimal? PromotionApplyQty2 { get; set; }
        public long? FkPromotionApplyUnitId { get; set; }
        public long? FKLotID { get; set; }//Hide FOr Nor

        public long? FkPromotionProdId { get; set; } //Free Product
        public decimal? PromotionAmt { get; set; }
        public decimal? PromotionQty { get; set; }
        public long? FkPromotionUnitId { get; set; }


        public long? FKProdID { get; set; }
        public long? FkProdCatgId { get; set; }
        public long? FkBrandId { get; set; }

        public string? CustomerName { get; set; }
        public string? VendorName { get; set; }
        public string? LocationName { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public string? PromotionProductName { get; set; }

        public long  SequenceNo { get; set; }

        //For extra
        public string? PromotionFromDt_str { get { return PromotionFromDt != null ? PromotionFromDt.Value.ToString("dd/MM/yyyy") : ""; } }
        public string? PromotionToDt_str { get { return PromotionToDt != null ? PromotionToDt.Value.ToString("dd/MM/yyyy") : ""; } }
        public string? PromotionFromTime_str { get { return !string.IsNullOrEmpty(PromotionFromTime) ? DateTime.ParseExact(PromotionFromTime, "H:mm", null, System.Globalization.DateTimeStyles.None).ToString("hh:mm tt") : ""; } }
        public string? PromotionToTime_str { get { return !string.IsNullOrEmpty(PromotionToTime) ? DateTime.ParseExact(PromotionToTime, "H:mm", null, System.Globalization.DateTimeStyles.None).ToString("hh:mm tt") : ""; } }

        public List<PromotionLocationLnkModel>? PromotionLocation_lst { get; set; }
        public List<PromotionLnkModel>? PromotionLnk_lst { get; set; }
       
    }

}
