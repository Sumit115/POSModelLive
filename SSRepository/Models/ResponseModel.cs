using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class ResponseModel
    {
        public string Response { get; set; }
        public long ID { get; set; }
    }

    public class ResModel
    {
        public string status { get; set; }
        public object data { get; set; }
        public string msg { get; set; }
    }
}
