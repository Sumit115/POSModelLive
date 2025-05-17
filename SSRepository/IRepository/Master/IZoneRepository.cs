
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IZoneRepository : IRepository<TblZoneMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(ZoneModel tblBankMas, string Mode);
        List<ZoneModel> GetList(int pageSize, int pageNo = 1, string search = "");
        ZoneModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
