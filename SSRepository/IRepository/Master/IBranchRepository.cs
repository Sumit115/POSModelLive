
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IBranchRepository : IRepository<TblBranchMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(BranchModel tblBankMas, string Mode);
        List<BranchModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "");
        BranchModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
