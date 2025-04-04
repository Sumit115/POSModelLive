using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository
{
    public interface ITranBaseRepository : IBaseRepository
    {
        string Create(TransactionModel model);
        DataTable GetList(string FromDate, string ToDate, string SeriesFilter, string DocumentType, string LocationFilter = "", string StateFilter = "");
        TransactionModel GetSingleRecord(long PkId, long FkSeriesId);
        object BarcodeScan(TransactionModel model, string barcode,bool isCalGridTotal);
        object ProductTouch(TransactionModel model, long PkProductId);
        object AutoFillLastRecord(TransactionModel model);

        object FooterChange(TransactionModel model, string fieldName);
        object PaymentDetail(TransactionModel model);

        object ApplyRateDiscount(TransactionModel model, string type, decimal discount);

        object ColumnChange(TransactionModel model, int rowIndex, string fieldName,bool IsReturn);
        List<ProdLotDtlModel> Get_ProductLotDtlList(int PKProductId, string Batch, string Color);

        List<ColumnStructure> ColumnList(string GridName = "");

        List<PartyModel> PartyList(int pageSize, int pageNo = 1, string search = "", string TranAlias = "");

        object SetParty(TransactionModel model, long FkPartyId);

        List<ProductModel> ProductList(int pageSize, int pageNo = 1, string search = "", long FkPartyId = 0, long FkInvoiceId = 0, DateTime? InvoiceDate = null);
        List<ProdLotDtlModel> ProductBatchList(int pageSize, int pageNo = 1, string search = "", long PKProductId = 0);
        List<ProdLotDtlModel> ProductColorList(int pageSize, int pageNo = 1, string search = "", long PKProductId = 0, string TranAlias = "", string Batch = "");
        List<ProdLotDtlModel> ProductMRPList(int pageSize, int pageNo = 1, string search = "", long PKProductId = 0, string Batch = "", string Color = "");

        object InvoiceList(long FkPartyId = 0, DateTime? InvoiceDate = null);

        List<BankModel> BankList();

        List<SeriesModel> SeriesList(int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "");

        object SetSeries(TransactionModel model, long FKSeriesId);
        object SetBankThroughBank(TransactionModel model, long FKBankThroughBankID);
        object SetLastSeries(TransactionModel model, long UserId, string TranAlias, string DocumentType);


        object VoucherColumnChange(TransactionModel model, int rowIndex, string fieldName);
        List<AccountMasModel> AccountList();

        object Get_CategorySizeList_ByProduct(long PKProductId, string search = "");
        long SaveWalkingCustomer(WalkingCustomerModel model);
        WalkingCustomerModel GeWalkingCustomer_byMobile(string Mobile);

        long GetIdbyEntryNo(long EntryNo, long FKSeriesId);
        object BarcodeList(TransactionModel model, int rowIndex, bool IsReturn);
        string SaveInvoiceBilty(long FkUserId,long FkID, long FKSeriesId, long FkFormId, string BiltyNo, string Image);
        object GetInvoiceBilty(long FkID, long FKSeriesId, long FkFormId);

        string SaveInvoiceShippingDetail(TransactionModel JsonData);
        object GetInvoiceShippingDetail(long FkID, long FKSeriesId);

        object GetPrintData(long PkId, long FkSeriesId);
        object ApplyPromotion(TransactionModel model);

    }
}
