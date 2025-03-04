using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSRepository.Data;
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
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomDropDownController(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _contextAccessor = contextAccessor;
        }

        [HttpPost]
        public object Usermas(int pageSize, int pageNo = 1, string search = "")
        {
            
            UserRepository repository = new UserRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo,  search);
        }

        [HttpPost]
        public object Branch(int pageSize, int pageNo = 1, string search = "")
        {

            BranchRepository repository = new BranchRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search);
        }

        [HttpPost]
        public object Station(int pageSize, int pageNo = 1, string search = "", long DistrictId = 0)
        {

            StationRepository repository = new StationRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search, DistrictId);
        }

        [HttpPost]
        public object Locality(int pageSize, int pageNo = 1, string search = "", long AreaId = 0)
        {

            LocalityRepository repository = new LocalityRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search, AreaId);
        }

        [HttpPost]
        public object Account(int pageSize, int pageNo = 1, string search = "")
        {

            AccountMasRepository repository = new AccountMasRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search);
        }

    }
}
