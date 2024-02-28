using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace SSRepository.Data
{
    [Table("tblProduct_mas", Schema = "dbo")]
    public partial class TblProductMas : TblBase, IEntity
    {
        [Key]
        public long PkProductId { get; set; }
        public string Product { get; set; }
        public string NameToDisplay { get; set; }
        public string NameToPrint { get; set; }
        public string? Image { get; set; }
        public string? Alias { get; set; }
        public string? ArticleType { get; set; }
        public string? ArticleNumber { get; set; }
        public string? Strength { get; set; }
        public string? Barcode { get; set; }
        public char Status { get; set; }
        public long FkCatGroupId { get; set; }
        public long FkCatId { get; set; }
        public long FKTaxID { get; set; }
        public string? HSNCode { get; set; }
        public long  FkBrandId { get; set; }
        public long FkUnitId { get; set; }
        public string? ShelfID { get; set; }
        public string? TradeDisc { get; set; }
        public int MinStock { get; set; }
        public int MaxStock { get; set; }
        public int MinDays { get; set; }
        public int MaxDays { get; set; }
        public int CaseLot { get; set; }
        public int BoxSize { get; set; }
        public string? Description { get; set; }
        public string? Unit1 { get; set; }
        public int ProdConv1 { get; set; }
        public string? Unit2 { get; set; }  
        public int ProdConv2 { get; set; } 
        public string? Unit3 { get; set; }
        public decimal MRP { get; set; }
        public decimal SaleRate { get; set; }
        public decimal TradeRate { get; set; }
        public decimal DistributionRate { get; set; }
        public decimal PurchaseRate { get; set; }
        public bool KeepStock { get; set; }

        [StringLength(20)]
        public string? Genration { get; set; }

        [StringLength(20)]
        public string? CodingScheme { get; set; }
    }
  
   



}

