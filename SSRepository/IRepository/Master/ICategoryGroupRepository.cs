
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ICategoryGroupRepository : IRepository<TblCategoryGroupMas>
    {
         List<ColumnStructure> ColumnList();

        string isAlreadyExist(CategoryGroupModel tblBankMas, string Mode);
        List<CategoryGroupModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpCategoryGroup(int pageSize, int pageNo = 1, string search = "");
        CategoryGroupModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
