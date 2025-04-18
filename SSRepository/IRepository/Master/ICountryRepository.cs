
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ICountryRepository : IRepository<TblCountryMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(CountryModel tblBankMas, string Mode);
        List<CountryModel> GetList(int pageSize, int pageNo = 1, string search = "");
         CountryModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
