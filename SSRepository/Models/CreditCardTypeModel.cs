using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    //replace by
    public class CreditCardTypeModel  : BaseModel
    {
        public long PKID { get; set; }
        public string? CreditCardType { get; set; }
        public long? FkAccountID { get; set; }
        public string? Assembly { get; set; }
        public string? Class { get; set; } 
        public string? Method { get; set; }
        public string? Parameter { get; set; }
        public string? AccountName { get; set; }
    }
}
