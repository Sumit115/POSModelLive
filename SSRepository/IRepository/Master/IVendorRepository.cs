
using SSRepository.Data;
using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository.Master
{
    public interface IVendorRepository : IRepository<TblVendorMas>
    {
        
        List<ColumnStructure> ColumnList();

        string isAlreadyExist(VendorModel tblBankMas, string Mode);
        List<VendorModel> GetList(int pageSize, int pageNo = 1, string search = "");
        VendorModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
        DataTable AutoDropDown();

    }
}
