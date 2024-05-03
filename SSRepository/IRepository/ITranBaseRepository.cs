using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository
{
    public interface ITranBaseRepository : IBaseRepository
    {
        string Create(TransactionModel model);
        DataTable GetList(string FromDate, string ToDate, string SeriesFilter = "");
        TransactionModel GetSingleRecord(long PkId, long FkSeriesId);
        object BarcodeScan(TransactionModel model, long barcode);
        object FooterChange(TransactionModel model, string fieldName);

        object ApplyRateDiscount(TransactionModel model, string type, decimal discount);

        object ColumnChange(TransactionModel model, int rowIndex, string fieldName);
        List<ProdLotDtlModel> Get_ProductLotDtlList(int PKProductId, string Batch, string Color);

        List<ColumnStructure> ColumnList(string GridName = "");

        List<PartyModel> PartyList(int pageSize, int pageNo = 1, string search = "", string TranType = "");

        object SetParty(TransactionModel model, long FkPartyId);

        List<ProductModel> ProductList();

        List<SeriesModel> SeriesList(int pageSize, int pageNo = 1, string search = "", string TranAlias = "");

        object SetSeries(TransactionModel model, long FKSeriesId);
        object SetLastSeries(TransactionModel model, long UserId, string TranAlias);

    }
}
