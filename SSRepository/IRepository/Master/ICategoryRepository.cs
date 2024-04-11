
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ICategoryRepository : IRepository<TblCategoryMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(CategoryModel tblBankMas, string Mode);
        List<CategoryModel> GetList(int pageSize, int pageNo = 1, string search = "", long CategoryGroupId = 0);
        CategoryModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
