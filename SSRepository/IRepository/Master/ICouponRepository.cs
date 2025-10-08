
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ICouponRepository : IRepository<TblCouponMas>
    {
         List<ColumnStructure> ColumnList(string GridName = ""); 
         List<CouponModel> GetList(int pageSize, int pageNo = 1, string search = "");
        CouponModel GetSingleRecord(long PkID);
       
        string DeleteRecord(long PKID);
    }
}
