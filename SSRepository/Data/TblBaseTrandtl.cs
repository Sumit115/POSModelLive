using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    public class TblBaseTrandtl : TblBase
    {
        public long FkId { get; set; }
        public long FKSeriesId { get; set; }
        public int sno { get; set; }

        public long FkProductId { get; set; }
        public long FkLotId { get; set; }
        public decimal MRP { get; set; }
        public decimal SaleRate { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal Rate { get; set; }
        public string RateUnit { get; set; }
        public decimal Qty { get; set; }
        public decimal FreeQty { get; set; }
        public decimal SchemeDisc { get; set; }
        public string SchemeDiscType { get; set; }
        public decimal SchemeDiscAmt { get; set; }
        public decimal TradeDisc { get; set; }
        public string TradeDiscType { get; set; }
        public decimal TradeDiscAmt { get; set; }
        public decimal LotDisc { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal ICRate { get; set; }
        public decimal ICAmt { get; set; }
        public decimal SCRate { get; set; }
        public decimal SCAmt { get; set; }
        public decimal NetAmt { get; set; }

        public string? Remark { get; set; }

    }
}
