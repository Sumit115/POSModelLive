
using SSRepository.Data;
using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository.Master
{
    public interface ICustomerRepository : IRepository<TblCustomerMas>
    {
      
        List<ColumnStructure> ColumnList();

        string isAlreadyExist(CustomerModel tblBankMas, string Mode);
        List<CustomerModel> GetList(int pageSize, int pageNo = 1, string search = "");
        CustomerModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
        DataTable AutoDropDown();

    }
}
