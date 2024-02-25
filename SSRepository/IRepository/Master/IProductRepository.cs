
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IProductRepository : IRepository<TblProductMas>
    {
          List<ColumnStructure> ColumnList();

        string isAlreadyExist(ProductModel tblBankMas, string Mode);
        List<ProductModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpProduct(int pageSize, int pageNo = 1, string search = "");
        ProductModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
