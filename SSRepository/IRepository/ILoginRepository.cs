
using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository
{

    public interface ILoginRepository:IBaseRepository
    {
        bool ValidateUser(long UserId);

        string UserMenu(long UserId);
        void Logout();
    }
}
