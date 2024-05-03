using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class TblProdStockDtlModel
    {
        public long PKStockId { get; set; }
        public long FKProdID { get; set; }
        public long FKLocationID { get; set; }
        public long FKLotID { get; set; }
        public decimal OpStock { get; set; }
        public decimal InStock { get; set; }
        public decimal OutStock { get; set; }
        public decimal CurStock { get; set; }
        public DateTime StockDate { get; set; }

        public string LocationName { get; set; }   
    }
}
