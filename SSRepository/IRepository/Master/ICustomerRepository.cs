
using SSRepository.Data;
using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository.Master
{
    public interface ICustomerRepository : IRepository<TblCustomerMas>
    {
      
        List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(PartyModel tblBankMas, string Mode);
        List<PartyModel> GetList(int pageSize, int pageNo = 1, string search = "");
        PartyModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
        DataTable AutoDropDown();

    }
}
