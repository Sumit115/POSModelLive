using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class TranDetails
    {
        // public long PkId { get; set; }
        public long FkId { get; set; }
        public long FKSeriesId { get; set; }//=0
        public int SrNo { get; set; }
        public int ModeForm { get; set; }

        public long FkProductId { get; set; }
        public long FKProdCatgId { get; set; }
        public long FkBrandId { get; set; }
        public long FkLotId { get; set; }//=0  

        //New Start 19-02-2024
        public string? Batch { get; set; }
        public string? Color { get; set; } //Only Purchase

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? MfgDate { get; set; }//Opt | Only Purchase

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; }//Opt | Only Purchase
        public decimal MRP { get; set; }
        public decimal? SaleRate { get; set; }//Only Purchase
        public decimal? TradeRate { get; set; }//Only Purchase
        public decimal? DistributionRate { get; set; }//Only Purchase
        //New End

        public decimal Rate { get; set; }//=Prodct Price according Series Selection
        public string? RateUnit { get; set; }//=''
        public decimal Qty { get; set; }//txt
        public decimal FreeQty { get; set; }//txt No Calculation Only stock Out
        public decimal SchemeDisc { get; set; }//=0
        public string? SchemeDiscType { get; set; }//=''
        public decimal SchemeDiscAmt { get; set; }//=0
        public decimal TradeDisc { get; set; }//=0
        public string? TradeDiscType { get; set; }//=''
        public decimal TradeDiscAmt { get; set; }//=0
        public decimal LotDisc { get; set; }//=0
        public string? LotDiscType { get; set; }//=''
        public decimal LotDiscAmt { get; set; }//=0
        public decimal GrossAmt { get; set; }//Product Taxable Amt//82
        public decimal ICRate { get; set; }
        public decimal ICAmt { get; set; }
        public decimal SCRate { get; set; }//9
        public decimal SCAmt { get; set; }//9
        public decimal NetAmt { get; set; }//=GrossAmt+ICAmt+SCAmt+SCAmt

        public string? Remark { get; set; }//=''

        //for extra Work
        public decimal GstRate { get; set; }//9
        public decimal GstAmt { get; set; }//9
        public string? Product { get; set; }
        public string? ProductDisplay { get; set; }
        public string? CodingScheme { get; set; }

        public Nullable<long> FKInvoiceID { get; set; }
        public Nullable<long> InvoiceSrNo { get; set; }
        public Nullable<long> FKInvoiceSrID { get; set; }
        public string? FKInvoiceID_Text { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }//Opt | Only Sale Return

        public long FKLocationID { get; set; }
        public long ReturnTypeID { get; set; }
        public decimal TaxableAmt { get; set; }//=Prodct Price according Series Selection


        public string? BrandName { get; set; }
        public string? HSNCode { get; set; }
        public string? Barcode { get; set; }
        public string? BarcodeTest { get; set; }

        public long DueQty { get; set; }
        public Nullable<long> FKOrderID { get; set; }
        public Nullable<long> OrderSrNo { get; set; }
        public Nullable<long> FKOrderSrID { get; set; }
        public string? SubCategoryName { get; set; }
        public long LinkSrNo { get; set; }
        public string PromotionType { get; set; } =string.Empty;
        public string? TranType { get; set; }
        public Nullable<long> FkPromotionId { get; set; }
        public string? PromotionName { get; set; } 


    }
}
