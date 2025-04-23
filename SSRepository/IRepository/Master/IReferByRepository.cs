
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IReferByRepository : IRepository<TblReferByMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(ReferByModel tblBankMas, string Mode);
        List<ReferByModel> GetList(int pageSize, int pageNo = 1, string search = "");
        ReferByModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
     }
}
