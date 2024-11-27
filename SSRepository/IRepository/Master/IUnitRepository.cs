
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IUnitRepository : IRepository<TblUnitMas>
    {
        
        List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(UnitModel tblUnitMas, string Mode);
        List<UnitModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpUnit(int pageSize, int pageNo = 1, string search = "");
        UnitModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
