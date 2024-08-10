
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ILocationRepository : IRepository<TblLocationMas>
    {
        List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(LocationModel obj, string Mode);
        List<LocationModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpLocation(int pageSize, int pageNo = 1, string search = "");
        LocationModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
