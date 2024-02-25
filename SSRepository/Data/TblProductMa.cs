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
        public string NameToDisplay { get; set; }//backend =product
        public string NameToPrint { get; set; }//backend =product
        public string? Image { get; set; }
        public string? Alias { get; set; }//=''
        public string? Strength { get; set; }//txt
        public string? Barcode { get; set; }//txt
        public char Status { get; set; }// Active=a / DeActive=d
        public long FkprodCatgId { get; set; }//ddl
        public long FKTaxID { get; set; }//=0
        public string HSNCode { get; set; }//txt
        public string Brand { get; set; }//txt
        public string? ShelfID { get; set; }//=''
        public string? TradeDisc { get; set; }//=''
        public int MinStock { get; set; }//txt
        public int MaxStock { get; set; }//txt
        public int MinDays { get; set; }//txt
        public int MaxDays { get; set; }//txt
        public int CaseLot { get; set; }//=0
        public int BoxSize { get; set; }//=0
        public string Description { get; set; }//txtaera
        public string? Unit1 { get; set; }//=''
        public int ProdConv1 { get; set; }//=0
        public string? Unit2 { get; set; }  //=''
        public int ProdConv2 { get; set; } //=0
        public string? Unit3 { get; set; }//=''
        public decimal MRP { get; set; }//txt
        public decimal SaleRate { get; set; }//txt
        public decimal TradeRate { get; set; }//txt
        public decimal DistributionRate { get; set; }//txt
        public decimal PurchaseRate { get; set; }//txt
        public bool KeepStock { get; set; }//=true 
    }
  
   



}

