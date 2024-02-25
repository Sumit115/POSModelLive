
namespace SSRepository.IRepository
{

    public interface IRepository<T>
    {
        public long FormID { get; }
        Task<string> CreateAsync(object tblmas, string Mode, Int64 ID, string dbType = "");

    }
    public interface IBaseRepository
    {      


    }
}
