
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IStateRepository : IRepository<TblStateMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(StateModel tblBankMas, string Mode);
        List<StateModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpState(int pageSize, int pageNo = 1, string search = "");
        object GetDrpTableState(int pageSize, int pageNo = 1, string search = "");

        object GetDrpStateByGroupId(long CountryId, int pageSize, int pageNo = 1, string search = "");
        StateModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
