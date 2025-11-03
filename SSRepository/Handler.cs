using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


public class Handler
{

    public enum en_src
    {
        SuperAdmin, //=0
        Admin,//=1
        User,//=2
        Branch,//=3
        Customer,//=4
        Employee,//5
    }
    public enum en_CustomFlag
    {
        CustomDrop = 100,
        Filter = 101
    }

    public enum Form
    {
        //MainMenu start from 1000
        Dashboard = 0,


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
        Location = 24,
        OpeningStock = 25,
        SalesPromotion = 26,
        PurchasePromotion = 27,
        Role = 28,
        Recipe = 29,
        Company = 30,
        Unit = 31,
        Form = 32,
        ReferBy = 33,
        CreditCardType = 34,
        Coupon = 35,

        SalesOrder = 100,
        SalesInvoice = 101,
        PurchaseOrder = 102,
        PurchaseInvoice = 103,
        SalesChallan = 104,
        SalesCrNote = 105,
        SalesReturn = 106,
        JournalVoucher = 107,
        ReceiptVoucher = 108,
        PaymentVoucher = 109,
        ContraVoucher = 110,
        WalkingSalesInvoice = 111,
        Voucher = 112,
        SalesInvoiceTouch = 113,
        LocationTransferRequest = 114,
        LocationTransferInvoice = 115,
        LocationRequest = 116,
        LocationReceive = 117,
        JobWork = 118,
        JobWorkIssue = 119,
        JobWorkReceive = 120,
        SalesReplacement = 121,
        Receipt = 122,

        //200-report
        SalesStock = 200,
        PurchaseStock = 201,
        StockDetail = 203,
        RateStock = 204,
        SalesTransaction = 205,
        PurchaseTransaction = 206,
        SalesOrderStock = 207,
        UniqueBarcodeTracking = 208,
        GSTReport = 209,
        AccountStatement = 210,
        StockAndSalesAnalysis = 211,
        WalkingCustomer = 212,
        WalkingCreditAmt = 213,

        //
        ImportStock = 300,

        //350-Option
        sysDefaults = 350,
        EntryLog = 351,


    }
    public enum AccountGroupMasterId
    {
        Sundry_Creditors = 1,
        Sundry_Debtors = 2,
        Purchase_Accounts = 3,
        Sales_Accounts = 4,
        Cash_In_Hand = 5,
        InDirect_Expenses = 6,
        Duties_And_Taxes = 7,
        Bank_Accounts = 8,
    }
    public enum AccountId
    {
        PURCHASE_TAXABLE_GOODS = 1,
        SALES_TAXABLE_GOODS = 2,
        SGST_INPUT = 3,
        SGST_OUTPUT = 4,
        CGST_INPUT = 5,
        CGST_OUTPUT = 6,
        IGST_INPUT = 7,
        IGST_OUTPUT = 8,
        CASH_IN_HAND = 9,
        BANK_ACCOUNTS = 10,
        ROUND_OFF_AC = 11,
    }
    public enum en_TranAlias
    {
        SINV,//Sales Invoice ,Walking Sales
        SORD,//Sales Order
        SPSL,//Sales Challan
        SRTN,//Sales Return
        SCRN,//Sales Cr Note
        SGRN,//Sales Replacement

        PINV,//Purchase Invoice
        PORD,//Purchase Order

        V_CT,//Contra Voucher
        V_JR,//Journal Voucher
        V_PY,//Payment Voucher
        V_RC,//Receipt Voucher 
    }
    public static List<ddl> GetDrpTranAlias()
    {
        return new List<ddl>(){
                   new ddl { Text = "Select", Value = "",Value2="" },
                   new ddl { Text = "Sales Order", Value = "SORD",Value2="" },
                   new ddl { Text = "Sales Invoice", Value = "SINV",Value2="" },
                   new ddl { Text = "Sales Challan", Value = "SPSL",Value2="SPSL" },
                   new ddl { Text = "Sales Return", Value = "SRTN",Value2="" },
                   new ddl { Text = "Sales Cr Note", Value = "SCRN",Value2="" },
                   new ddl { Text = "Purchase Order", Value = "PORD",Value2="" },
                   new ddl { Text = "Purchase Invoice", Value = "PINV",Value2="" },
                   new ddl { Text = "Contra Voucher", Value = "V_CT",Value2="" },
                   new ddl { Text = "Journal Voucher", Value = "V_JR",Value2="" },
                   new ddl { Text = "Payment Voucher", Value = "V_PY",Value2="" },
                   new ddl { Text = "Receipt Voucher", Value = "V_RC",Value2="" },
                   new ddl { Text = "Location Request", Value = "LORD",Value2="" },
                   new ddl { Text = "Location Invoice", Value = "LINV",Value2="" },
                   new ddl { Text = "Job Order", Value = "PJ_O",Value2="" },
                   new ddl { Text = "Job Issue", Value = "PJ_I",Value2="" },
                   new ddl { Text = "Job Receive", Value = "PJ_R",Value2="" },
            };
    }
    public static string GetTranAliasName(string TranAlias)
    {
        var _state = GetDrpTranAlias().ToList().Where(x => x.Value == TranAlias).FirstOrDefault();
        return _state != null ? _state.Text : "";
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

    public static string ConvertNumbertoWords(Int64 rup)
    {
        string result = "";
        Int64 res;
        if ((rup / 10000000) > 0)
        {
            res = rup / 10000000;
            rup = rup % 10000000;
            result = result + ' ' + RupeesToWords(res) + " Crore";
        }
        if ((rup / 100000) > 0)
        {
            res = rup / 100000;
            rup = rup % 100000;
            result = result + ' ' + RupeesToWords(res) + " Lakh";
        }
        if ((rup / 1000) > 0)
        {
            res = rup / 1000;
            rup = rup % 1000;
            result = result + ' ' + RupeesToWords(res) + " Thousand";
        }
        if ((rup / 100) > 0)
        {
            res = rup / 100;
            rup = rup % 100;
            result = result + ' ' + RupeesToWords(res) + " Hundred";
        }
        if ((rup % 10) >= 0)
        {
            res = rup % 100;
            result = result + " " + RupeesToWords(res);
        }
        result = result + " Rupees only";
        return result;
    }
    public static string RupeesToWords(Int64 rup)
    {
        string result = "";
        if ((rup >= 1) && (rup <= 10))
        {
            if ((rup % 10) == 1) result = "One";
            if ((rup % 10) == 2) result = "Two";
            if ((rup % 10) == 3) result = "Three";
            if ((rup % 10) == 4) result = "Four";
            if ((rup % 10) == 5) result = "Five";
            if ((rup % 10) == 6) result = "Six";
            if ((rup % 10) == 7) result = "Seven";
            if ((rup % 10) == 8) result = "Eight";
            if ((rup % 10) == 9) result = "Nine";
            if ((rup % 10) == 0) result = "Ten";
        }
        if (rup > 9 && rup < 20)
        {
            if (rup == 11) result = "Eleven";
            if (rup == 12) result = "Twelve";
            if (rup == 13) result = "Thirteen";
            if (rup == 14) result = "Forteen";
            if (rup == 15) result = "Fifteen";
            if (rup == 16) result = "Sixteen";
            if (rup == 17) result = "Seventeen";
            if (rup == 18) result = "Eighteen";
            if (rup == 19) result = "Nineteen";
        }
        if (rup > 20 && (rup / 10) == 2 && (rup % 10) == 0) result = "Twenty";
        if (rup > 20 && (rup / 10) == 3 && (rup % 10) == 0) result = "Thirty";


        if (rup > 20 && (rup / 10) == 4 && (rup % 10) == 0) result = "Forty";


        if (rup > 20 && (rup / 10) == 5 && (rup % 10) == 0) result = "Fifty";
        if (rup > 20 && (rup / 10) == 6 && (rup % 10) == 0) result = "Sixty";
        if (rup > 20 && (rup / 10) == 7 && (rup % 10) == 0) result = "Seventy";
        if (rup > 20 && (rup / 10) == 8 && (rup % 10) == 0) result = "Eighty";
        if (rup > 20 && (rup / 10) == 9 && (rup % 10) == 0) result = "Ninty";

        if (rup > 20 && (rup / 10) == 2 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Twenty One";
            if ((rup % 10) == 2) result = "Twenty Two";
            if ((rup % 10) == 3) result = "Twenty Three";
            if ((rup % 10) == 4) result = "Twenty Four";
            if ((rup % 10) == 5) result = "Twenty Five";
            if ((rup % 10) == 6) result = "Twenty Six";
            if ((rup % 10) == 7) result = "Twenty Seven";
            if ((rup % 10) == 8) result = "Twenty Eight";
            if ((rup % 10) == 9) result = "Twenty Nine";
        }
        if (rup > 20 && (rup / 10) == 3 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Thirty One";
            if ((rup % 10) == 2) result = "Thirty Two";
            if ((rup % 10) == 3) result = "Thirty Three";
            if ((rup % 10) == 4) result = "Thirty Four";
            if ((rup % 10) == 5) result = "Thirty Five";
            if ((rup % 10) == 6) result = "Thirty Six";
            if ((rup % 10) == 7) result = "Thirty Seven";
            if ((rup % 10) == 8) result = "Thirty Eight";
            if ((rup % 10) == 9) result = "Thirty Nine";
        }
        if (rup > 20 && (rup / 10) == 4 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Forty One";
            if ((rup % 10) == 2) result = "Forty Two";
            if ((rup % 10) == 3) result = "Forty Three";
            if ((rup % 10) == 4) result = "Forty Four";
            if ((rup % 10) == 5) result = "Forty Five";
            if ((rup % 10) == 6) result = "Forty Six";
            if ((rup % 10) == 7) result = "Forty Seven";
            if ((rup % 10) == 8) result = "Forty Eight";
            if ((rup % 10) == 9) result = "Forty Nine";
        }
        if (rup > 20 && (rup / 10) == 5 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Fifty One";
            if ((rup % 10) == 2) result = "Fifty Two";
            if ((rup % 10) == 3) result = "Fifty Three";
            if ((rup % 10) == 4) result = "Fifty Four";
            if ((rup % 10) == 5) result = "Fifty Five";
            if ((rup % 10) == 6) result = "Fifty Six";
            if ((rup % 10) == 7) result = "Fifty Seven";
            if ((rup % 10) == 8) result = "Fifty Eight";
            if ((rup % 10) == 9) result = "Fifty Nine";
        }
        if (rup > 20 && (rup / 10) == 6 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Sixty One";
            if ((rup % 10) == 2) result = "Sixty Two";
            if ((rup % 10) == 3) result = "Sixty Three";
            if ((rup % 10) == 4) result = "Sixty Four";
            if ((rup % 10) == 5) result = "Sixty Five";
            if ((rup % 10) == 6) result = "Sixty Six";
            if ((rup % 10) == 7) result = "Sixty Seven";
            if ((rup % 10) == 8) result = "Sixty Eight";
            if ((rup % 10) == 9) result = "Sixty Nine";
        }
        if (rup > 20 && (rup / 10) == 7 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Seventy One";
            if ((rup % 10) == 2) result = "Seventy Two";
            if ((rup % 10) == 3) result = "Seventy Three";
            if ((rup % 10) == 4) result = "Seventy Four";
            if ((rup % 10) == 5) result = "Seventy Five";
            if ((rup % 10) == 6) result = "Seventy Six";
            if ((rup % 10) == 7) result = "Seventy Seven";
            if ((rup % 10) == 8) result = "Seventy Eight";
            if ((rup % 10) == 9) result = "Seventy Nine";
        }
        if (rup > 20 && (rup / 10) == 8 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Eighty One";
            if ((rup % 10) == 2) result = "Eighty Two";
            if ((rup % 10) == 3) result = "Eighty Three";
            if ((rup % 10) == 4) result = "Eighty Four";
            if ((rup % 10) == 5) result = "Eighty Five";
            if ((rup % 10) == 6) result = "Eighty Six";
            if ((rup % 10) == 7) result = "Eighty Seven";
            if ((rup % 10) == 8) result = "Eighty Eight";
            if ((rup % 10) == 9) result = "Eighty Nine";
        }
        if (rup > 20 && (rup / 10) == 9 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Ninty One";
            if ((rup % 10) == 2) result = "Ninty Two";
            if ((rup % 10) == 3) result = "Ninty Three";
            if ((rup % 10) == 4) result = "Ninty Four";
            if ((rup % 10) == 5) result = "Ninty Five";
            if ((rup % 10) == 6) result = "Ninty Six";
            if ((rup % 10) == 7) result = "Ninty Seven";
            if ((rup % 10) == 8) result = "Ninty Eight";
            if ((rup % 10) == 9) result = "Ninty Nine";
        }
        return result;
    }

    public static List<ddl> GetDrpState(bool Isddl = true)
    {
        var list = new List<ddl>();
        if (Isddl)
        {
            list.Add(new ddl { Text = "Select", Value = "", Value2 = "" });
        }
        list.Add(new ddl { Text = "Andhra Pradesh", Value = "Andhra Pradesh", Value2 = "28" });
        list.Add(new ddl { Text = "Arunachal Pradesh", Value = "Arunachal Pradesh", Value2 = "12" });
        list.Add(new ddl { Text = "Assam", Value = "Assam", Value2 = "18" });
        list.Add(new ddl { Text = "Bihar", Value = "Bihar", Value2 = "10" });
        list.Add(new ddl { Text = "Chhattisgarh", Value = "Chhattisgarh", Value2 = "22" });
        list.Add(new ddl { Text = "Goa", Value = "Goa", Value2 = "30" });
        list.Add(new ddl { Text = "Gujarat", Value = "Gujarat", Value2 = "24" });
        list.Add(new ddl { Text = "Haryana", Value = "Haryana", Value2 = "06" });
        list.Add(new ddl { Text = "Himachal Pradesh", Value = "Himachal Pradesh", Value2 = "02" });
        list.Add(new ddl { Text = "Jharkhand", Value = "Jharkhand", Value2 = "20" });
        list.Add(new ddl { Text = "Karnataka", Value = "Karnataka", Value2 = "29" });
        list.Add(new ddl { Text = "Kerala", Value = "Kerala", Value2 = "32" });
        list.Add(new ddl { Text = "Madhya Pradesh", Value = "Madhya Pradesh", Value2 = "23" });
        list.Add(new ddl { Text = "Maharashtra", Value = "Maharashtra", Value2 = "27" });
        list.Add(new ddl { Text = "Manipur", Value = "Manipur", Value2 = "14" });
        list.Add(new ddl { Text = "Meghalaya", Value = "Meghalaya", Value2 = "17" });
        list.Add(new ddl { Text = "Mizoram", Value = "Mizoram", Value2 = "15" });
        list.Add(new ddl { Text = "Nagaland", Value = "Nagaland", Value2 = "13" });
        list.Add(new ddl { Text = "Odisha", Value = "Odisha", Value2 = "21" });
        list.Add(new ddl { Text = "Punjab", Value = "Punjab", Value2 = "03" });
        list.Add(new ddl { Text = "Rajasthan", Value = "Rajasthan", Value2 = "08" });
        list.Add(new ddl { Text = "Sikkim", Value = "Sikkim", Value2 = "11" });
        list.Add(new ddl { Text = "Tamil Nadu", Value = "Tamil Nadu", Value2 = "33" });
        list.Add(new ddl { Text = "Telangana", Value = "Telangana", Value2 = "36" });
        list.Add(new ddl { Text = "Tripura", Value = "Tripura", Value2 = "16" });
        list.Add(new ddl { Text = "Uttar Pradesh", Value = "Uttar Pradesh", Value2 = "09" });
        list.Add(new ddl { Text = "Uttarakhand", Value = "Uttarakhand", Value2 = "05" });
        list.Add(new ddl { Text = "West Bengal", Value = "West Bengal", Value2 = "19" });


        return list;
    }
    public static string GetStateCode(string StateName)
    {
        var _state = GetDrpState().ToList().Where(x => x.Value == StateName).FirstOrDefault();
        return _state != null ? _state.Value2 : "";
    }

    public static void Log(string title, string Des)
    { 
        string path = Path.Combine("wwwroot", "Logs");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        DateTime CurrentDateTime = DateTime.Now;
        string sPath = Path.Combine(path, "log_" + CurrentDateTime.ToString("ddMMyy") + ".txt");

        System.IO.File.AppendAllText(sPath, "\r\n\r\n" + CurrentDateTime.ToString("HH:mm:ss fff") + " [" + title + "] : " + Des);
    }
}

