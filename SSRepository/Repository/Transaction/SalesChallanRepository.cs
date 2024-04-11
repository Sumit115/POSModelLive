using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;
using SSRepository.Repository.Master;

namespace SSRepository.Repository.Transaction
{
    public class SalesChallanRepository : SalesInvoiceRepository, ISalesChallanRepository
    {
        public SalesChallanRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

    }
}
