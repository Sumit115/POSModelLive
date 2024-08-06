
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IPromotionRepository : IRepository<TblPromotionMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(PromotionModel tblBankMas, string Mode);
        List<PromotionModel> GetList(int pageSize, int pageNo = 1, string search = "");
        PromotionModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
