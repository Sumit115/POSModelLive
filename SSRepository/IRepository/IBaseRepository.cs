
using SSRepository.Data;

namespace SSRepository.IRepository
{

    public interface IRepository<T> : IBaseRepository where T : class, IEntity
    {
        
        Task<string> CreateAsync(object tblmas, string Mode, Int64 ID, string dbType = "");
        object GetDrpState();
       

    }
    public interface IBaseRepository
    {
        string GetSysDefaultsByKey(string SysDefKey);
    }
}
