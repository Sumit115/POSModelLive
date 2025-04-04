﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System.Data;
using System.Xml.Linq;

namespace SSRepository.Repository.Transaction
{
    public class LocationRequestRepository : SalesInvoiceRepository, ILocationRequestRepository
    {
        public LocationRequestRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            //SPAddUpd = "usp_LocationRequestAddUpd";
            SPList = "usp_LocationRequestList";
             SPById = "usp_LocationRequestById";
        }
         
    }
}
