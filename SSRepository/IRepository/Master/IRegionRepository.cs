
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IRegionRepository : IRepository<TblRegionMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(RegionModel tblBankMas, string Mode);
        List<RegionModel> GetList(int pageSize, int pageNo = 1, string search = "",long FkZoneId=0);
       
        RegionModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
