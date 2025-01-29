
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IFormRepository : IRepository<TblFormMas>
    {
        
        List<ColumnStructure> ColumnList(string GridName = "");

        List<FormModel> GetList(int pageSize, int pageNo = 1, string search = "");
        FormModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
