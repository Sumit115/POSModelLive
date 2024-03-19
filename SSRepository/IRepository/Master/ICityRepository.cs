
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ICityRepository : IRepository<TblCityMas>
    {
        
        List<ColumnStructure> ColumnList(string GridName = ""); 
        string isAlreadyExist(CityModel tblCityMas, string Mode);
        List<CityModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpCity(int pageSize, int pageNo = 1, string search = "");
        object GetDrpCity_ByState(string StateName);
        CityModel GetSingleRecord(long PkID); 
        string DeleteRecord(long PKID);
    }
}
