
using SSRepository.Models;

namespace SSRepository.IRepository
{

    public interface ILoginRepository:IBaseRepository
    {
        UserModel? Login(string UserId, string Pwd);
        UserModel  LoginV2(string UserId, string Pwd);
        void Logout();
    }
}
