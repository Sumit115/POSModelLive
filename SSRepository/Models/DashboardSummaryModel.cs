﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class DashboardSummaryModel
    {
        public DashboardSummaryModel()
        {
            CurrentMonthData = new List<CurrentMonthDataModel>();
        }
        public Int64 TotalCount_PurchaseInvoice { get; set; }
        public decimal TotalAmount_PurchaseInvoice { get; set; }
        public Int64 TotalCount_SalesOrder { get; set; }
        public decimal TotalAmount_SalesOrder { get; set; }
        public Int64 TotalCount_SalesInvoice { get; set; }
        public decimal TotalAmount_SalesInvoice { get; set; }
        public Int64 TotalCount_SalesChallan { get; set; }
        public decimal TotalAmount_SalesChallan { get; set; }
        public List<CurrentMonthDataModel> CurrentMonthData { get; set; }
    }
    public class CurrentMonthDataModel
    {
        public DateTime Date { get; set; }
        public string DateStr { get { return Date.ToString("yyyy-dd-MM"); } }

        public Int64 PurchaseInvoiceCount { get; set; }
        public decimal PurchaseInvoiceAmount { get; set; }
        public Int64 SalesInvoiceCount { get; set; }
        public decimal SalesInvoiceAmount { get; set; }
    }
}