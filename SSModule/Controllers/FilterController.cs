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
    public class FilterController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _contextAccessor;

        public FilterController(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _contextAccessor = contextAccessor;
        }

        [HttpPost]
        public object Series(int pageSize, int pageNo = 1, string search = "")
        { 
            SeriesRepository repository = new SeriesRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.Filter, pageSize, pageNo, search, "", "");
        }

        [HttpPost]
        public object State(int pageSize, int pageNo = 1, string search = "")
        {
             return Handler.GetDrpState(false);
        }

        [HttpPost]
        public object Location(int pageSize, int pageNo = 1, string search = "")
        {
            LocationRepository repository = new LocationRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.Filter, pageSize, pageNo, search);
        }
        [HttpPost]
        public object Vendor(int pageSize, int pageNo = 1, string search = "")
        {
            VendorRepository repository = new VendorRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.Filter, pageSize, pageNo, search);
        }
        [HttpPost]
        public object Customer(int pageSize, int pageNo = 1, string search = "")
        {
            CustomerRepository repository = new CustomerRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.Filter, pageSize, pageNo, search);
        }

        [HttpPost]
        public object Product(int pageSize, int pageNo = 1, string search = "")
        {
            ProductRepository repository = new ProductRepository(_dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.Filter, pageSize, pageNo, search);
        }
 
    }
}
