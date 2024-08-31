
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface IRecipeRepository : IRepository<TblRecipeMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(RecipeModel tblBankMas, string Mode);
        List<RecipeModel> GetList(int pageSize, int pageNo = 1, string search = "", long RecipeGroupId = 0);
        RecipeModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
