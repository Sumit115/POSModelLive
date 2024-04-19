
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IDistrictRepository : IRepository<TblDistrictMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(DistrictModel tblBankMas, string Mode);
        List<DistrictModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpDistrict(int pageSize, int pageNo = 1, string search = "");
        object GetDrpDistrictByStateId(long StateId, int pageSize, int pageNo = 1, string search = "");
        object GetDrpTableDistrict(int pageSize, int pageNo = 1, string search = "");
        DistrictModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
