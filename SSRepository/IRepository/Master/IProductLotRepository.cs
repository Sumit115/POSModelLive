
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IProductLotRepository : IRepository<TblProdLotDtl>
    {

        List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(ProdLotDtlModel model, string Mode);
        List<ProdLotDtlModel> GetList(int pageSize, int pageNo = 1, string search = "");
        List<ProdLotDtlModel> GetListByProduct(long FkProductId, int pageSize, int pageNo = 1, string search = "");
        object GetDrpProdLotDtl(int pageSize, int pageNo = 1, string search = "");
        ProdLotDtlModel GetSingleRecord(long PkID);
        string UpdateProdLotDtl(long PkLotId, long FKProductId, string ColumnName, decimal Value);

        string DeleteRecord(long PKID);
    }
}
