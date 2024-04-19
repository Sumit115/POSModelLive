using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.IO;
using System.Net; 
using System.Text;
using System.Data.OleDb;
using SSRepository;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Reflection;

namespace SSAdmin
{
    public class Handler : Controller
    {
        public int LoginId
        {
            get
            {
                return Convert.ToInt32(HttpContext.Session.GetString("LoginId"));
            }
        }
        public int src_Id
        {
            get
            {
                return Convert.ToInt32(HttpContext.Session.GetString("LoginId"));
            }
        }
        public en_src src
        {
            get
            {
                return Enum.Parse<en_src>(HttpContext.Session.GetString("src")); ;
            }
        }
        public enum en_src
        {
            SuperAdmin, //=0
            Admin,//=1
            User,//=2
            Branch,//=3
            Customer,//=4
            Employee,//5
        }
        

        public enum en_PayMode
        {
            Cash,
            Wallet,
            Paytm,
            Razorpay,
        }
        public enum Form
        {
            Customer = 1,
            Vendor = 2,
            Employee = 3,
            Branch = 4,
            User = 5,
            Category = 6,
            Product = 7,
            Bank = 8,
            Series = 9,
            CategoryGroup = 10,
            Brand = 11,
            City = 12,
            ProductLot = 13,
            AccountGroup = 14,
            AccountMas = 15,
            Country = 16,
            State = 17,
            District = 18,
            Station = 19,
            Zone = 20,
            Region = 21,
            Area = 22,
            Locality = 23,

            SalesOrder = 100,
            SalesInvoice = 101,
            PurchaseOrder = 102,
            PurchaseInvoice = 103,
            SalesChallan = 104,

            SalesStock = 200,
            PurchaseStock = 200,

            //200-report
        }

        public static bool IsMobileNumber(string number)
        {
            return Regex.Match(number, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$").Success;
        }

        public static DataTable GetDataTableFromObjects(object o)
        {
            Type t = o.GetType();
            DataTable dt = new DataTable(t.Name);
            foreach (System.Reflection.PropertyInfo pi in t.GetProperties())
            {
                //dt.Columns.Add(new DataColumn(pi.Name, Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType));
                var column = new DataColumn
                {
                    ColumnName = pi.Name,
                    DataType = pi.PropertyType.Name.Contains("Nullable") ? typeof(string) : pi.PropertyType
                };

                dt.Columns.Add(column);
            }

            DataRow dr = dt.NewRow();
            foreach (DataColumn dc in dt.Columns)
            {
                dr[dc.ColumnName] = o.GetType().GetProperty(dc.ColumnName).GetValue(o, null);
            }
            dt.Rows.Add(dr);
            return dt;
        }

        public static DataTable ObjectToDataTable(object o)
        {
            DataTable dt = new DataTable("OutputData");
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            o.GetType().GetProperties().ToList().ForEach(f =>
            {
                try
                {
                    f.GetValue(o, null);
                    dt.Columns.Add(f.Name, f.PropertyType);
                    dt.Rows[0][f.Name] = f.GetValue(o, null);
                }
                catch { }
            });
            return dt;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static DataTable GenerateExcel(DataTable _gridColumn, DataTable data)
        {

            string ColHead = "";
            string ColField = "";
            foreach (DataRow dr in _gridColumn.Rows)
            {
                if (_gridColumn.Columns.Contains("field"))
                {
                    if (data.Columns.Contains(dr["field"].ToString()))
                    {
                        ColHead += dr["name"].ToString() + ",";
                        ColField += dr["field"].ToString() + ",";
                    }
                }
                else
                {
                    if (data.Columns.Contains(dr["Fields"].ToString()))
                    {
                        ColHead += dr["Heading"].ToString() + ",";
                        ColField += dr["Fields"].ToString() + ",";
                    }
                }
            }
            if (!string.IsNullOrEmpty(ColHead))
            {
                ColHead = ColHead.Substring(0, ColHead.Length - 1);
                ColField = ColField.Substring(0, ColField.Length - 1);
            }

            string[] arrColHead = ColHead.Split(',');
            string[] arrColField = ColField.Split(',');
            System.Data.DataView view = new System.Data.DataView(data);
            System.Data.DataTable selected = view.ToTable("Selected", false, arrColField);
            for (int i = 0; i < arrColHead.Length; i++)
            {
                selected.Columns[arrColField[i]].ColumnName = arrColHead[i];
                selected.AcceptChanges();
            }

            return selected;

        }

    }
}