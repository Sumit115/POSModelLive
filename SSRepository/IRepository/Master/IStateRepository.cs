
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IStateRepository : IRepository<TblStateMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(StateModel tblBankMas, string Mode);
        List<StateModel> GetList(int pageSize, int pageNo = 1, string search = "", long FkCountryId = 0);
 
        StateModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
