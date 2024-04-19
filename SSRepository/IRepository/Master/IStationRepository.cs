
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IStationRepository : IRepository<TblStationMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(StationModel tblBankMas, string Mode);
        List<StationModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpStation(int pageSize, int pageNo = 1, string search = "");
        object GetDrpStationByDistrictId(long DistrictId, int pageSize, int pageNo = 1, string search = "");
        StationModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
