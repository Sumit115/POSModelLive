
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IAreaRepository : IRepository<TblAreaMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(AreaModel tblBankMas, string Mode);
        List<AreaModel> GetList(int pageSize, int pageNo = 1, string search = "",long FkRegionId=0);
      
        AreaModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
