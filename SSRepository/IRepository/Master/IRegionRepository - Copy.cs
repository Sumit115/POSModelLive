
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ILocalityRepository : IRepository<TblLocalityMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(LocalityModel tblBankMas, string Mode);
        List<LocalityModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpLocality(int pageSize, int pageNo = 1, string search = "");
        object GetDrpLocalityByAreaId(long AreaId, int pageSize, int pageNo = 1, string search = "");
        object GetDrpTableLocality(int pageSize, int pageNo = 1, string search = "");
        LocalityModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
