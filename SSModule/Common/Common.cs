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
        public enum en_rec_for
        {
            Admin,
            User,
            Franchise,
            Employee,
            Customer,
            ECommerce
        }
        public enum en_formMasterId
        {
            Null, //=0
            Employee,//=1
            User,//=2
            Table,//=3
            TableArea,//=4
            Category,//=5
            Variation,//6
            Addon,//7
            Product,//8
        }
        public static bool IsMobileNumber(string number)
        {
            return Regex.Match(number, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$").Success;
        }
    }
}