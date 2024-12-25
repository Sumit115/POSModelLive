using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System.Data;
using System.Xml.Linq;

namespace SSRepository.Repository.Transaction
{
    public class LocationTransferRequestRepository : SalesOrderRepository, ILocationTransferRequestRepository
    {
        public LocationTransferRequestRepository(AppDbContext dbContext) : base(dbContext)
        {
            //SPAddUpd = "usp_LocationTransferRequestAddUpd";
            //SPList = "usp_LocationTransferRequestList";
            //SPById = "usp_LocationTransferRequestById";
        }
         
    }
}
