using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class AccountGroupModel : BaseModel
    { 
        public long PkAccountGroupId { get; set; }
        public long? FkAccountGroupId { get; set; }
        public string? GroupType { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(50)]
        public string AccountGroupName { get; set; }
        public string? GroupAlias { get; set; }
        public string? NatureOfGroup { get; set; }
        public bool PrintDtl { get; set; }
        public bool NetCrDrBalanceForRpt { get; set; }

        //Extra
        public string? PAccountGroupName { get; set; }
        public string? GroupType_FullName
        {
            get
            {
                var t = "";
                if (GroupType == "B") { t = "Balance Sheet"; }
                else if (GroupType == "P") { t = "Profit & Loss"; }
                else if (GroupType == "T") { t = "Trading"; }  
                return t;
            }
        }
        public string? NatureOfGroup_FullName
        {
            get
            {
                var t = "";
                if (NatureOfGroup == "A") { t = "Assests"; }
                else if (NatureOfGroup == "L") { t = "Liabilities"; }
                else if (NatureOfGroup == "I") { t = "Income"; }
                else if (NatureOfGroup == "E") { t = "Expenses"; } 
                return t;
            }
        }
        public string? PrintDtl_Status
        {
            get
            {
                 return PrintDtl?"YES":"NO";
            }
        }
        public string? NetCrDrBalanceForRpt_Status
        {
            get
            {
                return NetCrDrBalanceForRpt ? "YES" : "NO";
            }
        }

    }
}
