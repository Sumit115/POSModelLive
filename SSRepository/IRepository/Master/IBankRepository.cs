
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IBankRepository : IRepository<TblBankMas>
    {
        
        List<ColumnStructure> ColumnList();

        string isAlreadyExist(BankModel tblBankMas, string Mode);
        List<BankModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpBank(int pageSize, int pageNo = 1, string search = "");
        BankModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
