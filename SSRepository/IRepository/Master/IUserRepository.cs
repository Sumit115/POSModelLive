
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IUserRepository : IRepository<TblUserMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");
        string isAlreadyExist(UserModel tblBankMas, string Mode);
        List<UserModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpUser(int pageSize, int pageNo = 1, string search = "");
        UserModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);

    }
}
