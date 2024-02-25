
using SSRepository.Models;

namespace SSRepository.IRepository
{

    public interface ILoginRepository
    {
        UserModel? Login(string UserId, string Pwd);
        void Logout();
    }
}
