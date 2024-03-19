
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IBrandRepository : IRepository<TblBrandMas>
    {
        
        List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(BrandModel tblBrandMas, string Mode);
        List<BrandModel> GetList(int pageSize, int pageNo = 1, string search = "");
        object GetDrpBrand(int pageSize, int pageNo = 1, string search = "");
        BrandModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
