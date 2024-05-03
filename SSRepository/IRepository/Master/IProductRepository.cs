
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IProductRepository : IRepository<TblProductMas>
    {
          List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(ProductModel tblBankMas, string Mode);
        List<ProductModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpProduct(int pageSize, int pageNo = 1, string search = "", long FkCatId = 0);
        ProductModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);

        List<CategoryModel> prodCatgList(int pageSize, int pageNo = 1, string search = "");

        string GetBarCode();
    }
}
