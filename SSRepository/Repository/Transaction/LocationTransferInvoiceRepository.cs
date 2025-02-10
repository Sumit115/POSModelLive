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
    public class LocationTransferInvoiceRepository : SalesInvoiceRepository, ILocationTransferInvoiceRepository
    {
        public LocationTransferInvoiceRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            //SPAddUpd = "usp_LocationTransferInvoiceAddUpd";
            //SPList = "usp_LocationTransferInvoiceList";
            //SPById = "usp_LocationTransferInvoiceById";
        }
         
    }
}
