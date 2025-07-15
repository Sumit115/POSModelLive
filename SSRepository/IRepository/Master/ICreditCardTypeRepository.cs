
using SSRepository.Data;
using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository.Master
{
    public interface ICreditCardTypeRepository : IRepository<TblCreditCardTypeMas>
    {
      
        List<ColumnStructure> ColumnList(string GridName = "");

        List<CreditCardTypeModel> GetList(int pageSize, int pageNo = 1, string search = "");
      
        CreditCardTypeModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);  
    }
}
