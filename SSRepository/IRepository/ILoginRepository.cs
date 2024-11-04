
using SSRepository.Models;

namespace SSRepository.IRepository
{

    public interface ILoginRepository:IBaseRepository
    {
        UserModel ValidateUser(long UserId);
        void Logout();
    }
}
