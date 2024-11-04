
using LMS.Models;

namespace LMS.IRepository
{

    public interface IAuthRepository
    {
        SignInValidate ValidateUser(SignInModel model);
        void Logout();
    }
}
