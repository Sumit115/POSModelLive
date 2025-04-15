
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
        SysDefaults ObjSysDefault { get; }
        string GetSysDefaultsByKey(string SysDefKey);
        List<SysDefaultsModel> GetSysDefaultsList(string search = "");
        SysDefaults GetSysDefaults();
        void UpdateSysDefaults(object objmodel);
        void InsertUpdateSysDefaults(string SysDefKey, string SysDefValue);
        void InsertUpdateSysDefaults(object objmodel);
        List<BarcodePrintPreviewModel> BarcodePrintList(List<BarcodeDetails> model);
        void UpdatePrintBarcode(object objmodel);
         public List<FormModel> GetFormList(long? FKMasterFormID = null);
        public int ExecNonQuery(string cmdText);
        DashboardSummaryModel usp_DashboardSummary(int Month);
        MasterLogDtlModel GetMasterLog(long PKMasterLogID);
        T GetMasterLog<T>(long PKMasterLogID);
        string GetAlias(string FormName = "");
    }
}
