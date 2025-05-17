
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IDistrictRepository : IRepository<TblDistrictMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(DistrictModel tblBankMas, string Mode);
        List<DistrictModel> GetList(int pageSize, int pageNo = 1, string search = "", long FkStateId = 0);
          DistrictModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
