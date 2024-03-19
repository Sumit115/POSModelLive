
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IEmployeeRepository : IRepository<TblEmployeeMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(EmployeeModel tblBankMas, string Mode);
        List<EmployeeModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpEmployee(int pageSize, int pageNo = 1, string search = "");
        EmployeeModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
     }
}
