using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System.Data;
using System.Xml.Linq;

namespace SSRepository.Repository.Transaction
{
    public class LocationReceiveRepository : SalesInvoiceRepository, ILocationReceiveRepository
    {
        public LocationReceiveRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            //SPAddUpd = "usp_LocationReceiveAddUpd";
            //SPList = "usp_LocationReceiveList";
            //SPById = "usp_LocationReceiveById";
        }
         
    }
}
