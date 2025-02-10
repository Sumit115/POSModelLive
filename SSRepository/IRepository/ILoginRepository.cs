
using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository
{

    public interface ILoginRepository:IBaseRepository
    {
        UserModel ValidateUser(long UserId);

        string UserMenu(long UserId);
        void Logout();
    }
}
