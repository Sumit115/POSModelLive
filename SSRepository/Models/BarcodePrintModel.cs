using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class BarcodePrintModel
    {
        public BarcodePrintModel()
        {
            BarcodeDetails = new List<BarcodeDetails>();
            SysDefaults = new List<SysDefaultsModel>();
            BarcodePrintPreviewModel = new List<BarcodePrintPreviewModel>();
        }

        public List<BarcodeDetails> BarcodeDetails { get; set; }
        public List<SysDefaultsModel> SysDefaults { get; set; }
        public List<BarcodePrintPreviewModel> BarcodePrintPreviewModel { get; set; }

    }

    public class BarcodeDetails
    {
        public long FKProductId { get; set; }
        public Nullable<long> FKLocationId { get; set; }
        public Nullable<long> TranInId { get; set; }
        public Nullable<long> TranInSeriesId { get; set; }
        public Nullable<long> TranInSrNo { get; set; }
        public Nullable<long> TranOutId { get; set; }
        public Nullable<long> TranOutSeriesId { get; set; }
        public Nullable<long> TranOutSrNo { get; set; }
        public long FkLocationId { get; set; }
    }
    public class BarcodePrintPreviewModel
    {
        public string Barcode { get; set; }
        public decimal MRP { get; set; }
        public Nullable<decimal> SaleRate { get; set; }
        public string Batch { get; set; }
        public string Product { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)] 
        public Nullable<System.DateTime> StockDate { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string Pin { get; set; }

    }
}
