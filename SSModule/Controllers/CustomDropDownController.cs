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
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search);
        }

        [HttpPost]
        public object Series(int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "")
        {

            SeriesRepository repository = new SeriesRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search, TranAlias, DocumentType);
        }
        [HttpPost]
        public object AccountGroup(int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "")
        {

            AccountGroupRepository repository = new AccountGroupRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search, TranAlias, DocumentType);
        }

        [HttpPost]
        public object Bank(int pageSize, int pageNo = 1, string search = "")
        { 
            BankRepository repository = new BankRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search);
        }

        [HttpPost]
        public object CategoryGroup(int pageSize, int pageNo = 1, string search = "")
        {

            CategoryGroupRepository repository = new CategoryGroupRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search);
        }


        //[HttpPost]
        //public object FKSeriesId(int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "")
        //{
        //    return _seriesRepository.CustomList(1, pageSize, pageNo, search, TranAlias, DocumentType);
        //}



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


        [HttpPost]
        public object Employee(int pageSize, int pageNo = 1, string search = "")
        {
            EmployeeRepository repository = new EmployeeRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search);
        }


        [HttpPost]
        public object Location(int pageSize, int pageNo = 1, string search = "")
        {

            LocationRepository repository = new LocationRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search);
        }

        [HttpPost]
        public object Role( int pageSize, int pageNo = 1, string search = "")
        {

            RoleRepository repository = new RoleRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search);
        }

    }


}