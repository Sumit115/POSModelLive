
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ISeriesRepository : IRepository<TblSeriesMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(SeriesModel tblBankMas, string Mode);
        List<SeriesModel> GetList(int pageSize, int pageNo = 1, string search = "", string TranAlias = "");
        SeriesModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
