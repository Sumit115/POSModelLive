using DocumentFormat.OpenXml.Wordprocessing;
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
        private readonly IUserRepository _userRepository;
        private readonly ISeriesRepository _seriesRepository;

        public CustomDropDownController( IUserRepository UserRepository, ISeriesRepository SeriesRepository)
        {
            _userRepository = UserRepository;
            _seriesRepository = SeriesRepository;
        }

        [HttpPost]
        public object FkUserId(int pageSize, int pageNo = 1, string search = "")
        {
            return _userRepository.GetDrpUser(pageSize, pageNo, search);
        }
        [HttpPost]
        public object FKSeriesId(int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "")
        { 
            return _seriesRepository.CustomList(1,pageSize, pageNo,  search, TranAlias, DocumentType);
        }

    }
}
