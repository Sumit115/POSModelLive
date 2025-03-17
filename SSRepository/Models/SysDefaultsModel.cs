using Microsoft.AspNetCore.Http;
using SSRepository.Data;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Models
{

    public class SysDefaultsModel
    {

        [Key]
        public long PKSysDefID { get; set; }
        public string SysDefKey { get; set; }
        public string SysDefValue { get; set; }
        public string FKTableName { get; set; }
        public string FKColumnName { get; set; }
        public long FKUserID { get; set; }
        public System.DateTime DATE_MODIFIED { get; set; }

    }

    //For Add Update Data
    public class SysDefaults
    {
        public string? CompanyName { get; set; }
        public string? CompanyContactPerson { get; set; }

        [EmailAddress]
        public string? CompanyEmail { get; set; }

        [Phone]
        public string? CompanyMobile { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyCityId { get; set; }
        public string? CompanyCity { get; set; }
        public string? CompanyState { get; set; }
        public string? CompanyPin { get; set; }
        public string? CompanyCountry { get; set; }
        public string? CompanyImage1 { get; set; }
        public IFormFile? MyCompanyImage1 { set; get; }
        public string? CodingScheme { get; set; }
        public string? BarcodePrint_Height { get; set; }
        public string? BarcodePrint_width { get; set; }
        public string? BarcodePrint_MarginTop { get; set; }
        public string? BarcodePrint_MarginBottom { get; set; }
        public string? BarcodePrint_MarginLeft { get; set; }
        public string? BarcodePrint_MarginRight { get; set; }
        public string? BarcodePrint_BarcodeHeight { get; set; }
        public string? BarcodePrint_ColumnInPerRow { get; set; }
        public string? BarcodePrint_MarginBetWeenRowColumn { get; set; }
        public string? BarcodePrint_FontSize { get; set; }

        public string? FkHoldLocationId { get; set; }
        public string? FinYear { get; set; }
        public string BillingLocation { get; set; }
        public string Location { get; set; }

        public long FkRoleId { get; set; }
        public int IsAdmin { get; set; }


    }
}
