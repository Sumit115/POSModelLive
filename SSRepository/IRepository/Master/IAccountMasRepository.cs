
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IAccountMasRepository : IRepository<TblAccountMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(AccountMasModel tblBankMas, string Mode);
        List<AccountMasModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpAccountMas(int pageSize, int pageNo = 1, string search = "");
        AccountMasModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
