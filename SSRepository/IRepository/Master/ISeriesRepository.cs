
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ISeriesRepository : IRepository<TblSeriesMas>
    {
         List<ColumnStructure> ColumnList();

        string isAlreadyExist(SeriesModel tblBankMas, string Mode);
        List<SeriesModel> GetList(int pageSize, int pageNo = 1, string search = "");
        List<SeriesModel> GetList_by_TranAlias(string TranAlias);
        object GetDrpSeries(int pageSize, int pageNo = 1, string search = "");
        SeriesModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
