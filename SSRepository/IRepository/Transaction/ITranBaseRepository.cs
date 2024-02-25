using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.IRepository.Transaction
{
    public interface ITranBaseRepository
    {
        public long FormID { get; }
        public long FormID_Create { get; }
        string Create(TranModel model); 
        DataTable GetList(string FromDate,string ToDate,string SeriesFilter="");
        TranModel GetSingleRecord(long PkId, long FkSeriesId);

        object ColumnChange(TranModel model, int rowIndex, string fieldName);
        object FooterChange(TranModel model, string fieldName);
        List<ColumnStructure> ColumnList_CreateTran(string TranType);
        List<ProdLotDtlModel> Get_ProductLotDtlList(int PKProductId);

    }
}
