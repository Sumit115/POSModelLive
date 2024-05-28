using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
     public class ProductModel : BaseModel
    {
         public long PkProductId { get; set; }
        public string Product { get; set; }
        public string? NameToDisplay { get; set; }//backend =product
        public string? NameToPrint { get; set; }//backend =product
        public string? Image { get; set; }
        public string? Alias { get; set; }//=''
        public string? Strength { get; set; }//txt
        public long Barcode { get; set; }//txt
        public string Status { get; set; }
        [Required]
        public long FkCatGroupId { get; set; }

        [Required]
        public long FKProdCatgId { get; set; }//ddl
        public long? FKTaxID { get; set; }//=0
        public string? HSNCode { get; set; }//txt
        public long? FkBrandId { get; set; }
        public long? FkUnitId { get; set; }//txt
        public string? ShelfID { get; set; }//=''
        public decimal TradeDisc { get; set; }

        public int? MinStock { get; set; }//txt
        public int? MaxStock { get; set; }//txt
        public int? MinDays { get; set; }//txt
        public int? MaxDays { get; set; }//txt
        public string CaseLot { get; set; }//=0
        public int? BoxSize { get; set; }//=0
        public string? Description { get; set; }//txtaera
        public string? Unit1 { get; set; }//=''
        public decimal ProdConv1 { get; set; }//=0
        public string? Unit2 { get; set; }  //=''
        public decimal? ProdConv2 { get; set; } //=0
        public string? Unit3 { get; set; }//=''
        public decimal MRP { get; set; }//txt
        public decimal SaleRate { get; set; }//txt
        public decimal TradeRate { get; set; }//txt
        public decimal DistributionRate { get; set; }//txt
        public decimal PurchaseRate { get; set; }//txt
        public bool KeepStock { get; set; }//=true 

        
        public string? CategoryGroupName { get; set; }
        public string? CategoryName { get; set; }// 
       public string? BrandName { get; set; }//

        [StringLength(20)]
        public string Genration { get; set; }

        [StringLength(20)]
        public string? CodingScheme { get; set; }

        public long? FKInvoiceID { get; set; }// 
        public long? InvoiceSrNo { get; set; }
        public long? FKInvoiceSrID { get; set; }
    }
}
