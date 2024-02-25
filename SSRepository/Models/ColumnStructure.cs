using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class ColumnStructure
    {public int pk_Id { get; set; }
        public string Heading { get; set; }
        public string Fields { get; set; }
        public int SearchType { get; set; }
        public int Sortable { get; set; }
        public string CtrlType { get; set; }
        public int Orderby { get; set; }
        public int Width { get; set; }
        public int IsActive { get; set; } 
    }

     
    }
