
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository
{

    public interface IRepository<T> : IBaseRepository where T : class, IEntity
    {

        Task<string> CreateAsync(object tblmas, string Mode, Int64 ID, string dbType = "");
        // object GetDrpState(); 
    }
    public interface IBaseRepository
    {
        string GetSysDefaultsByKey(string SysDefKey);
        List<SysDefaultsModel> GetSysDefaultsList(string search = "");
        void UpdateSysDefaults(object objmodel);
         List<BarcodePrintPreviewModel>    BarcodePrintList(List<BarcodeDetails> model);
        void UpdatePrintBarcode(object objmodel);
        List<ddl> UnitList();
      
    }
}
