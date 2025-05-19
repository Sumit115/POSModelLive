using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.IRepository.Report
{
    public interface IUniqueBarcodeTrackingRepository : IReportBaseRepository
    {
        DataTable GetList(string Barcode = "", string ProductFilter = "", string SaleSeriesFilter = "", string SaleEntryNoFrom = "", string SaleEntryNoTo = "", string SaleDateFrom = "", string SaleDateTo = "", string PurchaseSeriesFilter = "", string PurchaseEntryNoFrom = "", string PurchaseEntryNoTo = "", string PurchaseDateFrom = "", string PurchaseDateTo = "");
    List<ColumnStructure> ColumnList(string GridName = "");
    }
}
