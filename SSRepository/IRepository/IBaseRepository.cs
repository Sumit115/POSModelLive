
namespace SSRepository.IRepository
{

    public interface IRepository<T>
    {
        public long FormID { get; }
        Task<string> CreateAsync(object tblmas, string Mode, Int64 ID, string dbType = "");
        object GetDrpState();

    }
    public interface IBaseRepository
    {      


    }
}
