
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{ 
    public interface IAccountGroupRepository : IRepository<TblAccountGroupMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(AccountGroupModel tblBankMas, string Mode);
        List<AccountGroupModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpAccountGroup(int pageSize, int pageNo = 1, string search = "");
        object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "");
        AccountGroupModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
