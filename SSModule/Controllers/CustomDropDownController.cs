using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System.Data;
using System.Text;

namespace SSAdmin.Controllers
{
    public class CustomDropDownController : Controller
    {
         private readonly IUserRepository _Userrepository;

        public CustomDropDownController( IUserRepository Userrepository)
        {
            _Userrepository = Userrepository; 
        }

        [HttpPost]
        public object FkUserId(int pageSize, int pageNo = 1, string search = "")
        {
            return _Userrepository.GetDrpUser(pageSize, pageNo, search);
        }

    }
}
