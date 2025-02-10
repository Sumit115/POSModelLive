
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IRoleRepository : IRepository<TblRoleMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(RoleModel tblBankMas, string Mode);
        List<RoleModel> GetList(int pageSize, int pageNo = 1, string search = "", long fkUserId = 0);
        object GetDrpRole(int pageSize, int pageNo = 1, string search = "");

        RoleModel GetSingleRecord(long PkID, bool IsAccess = false);

        string DeleteRecord(long PKID);
    }
}
