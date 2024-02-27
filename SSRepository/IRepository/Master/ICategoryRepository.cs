
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ICategoryRepository : IRepository<TblCategoryMas>
    {
         List<ColumnStructure> ColumnList();

        string isAlreadyExist(CategoryModel tblBankMas, string Mode);
        List<CategoryModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpCategory(int pageSize, int pageNo = 1, string search = "");
        object GetDrpCategoryByGroupId(long CategoryGroupId, int pageSize, int pageNo = 1, string search = "");
        CategoryModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
