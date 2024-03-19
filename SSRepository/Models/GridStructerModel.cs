using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class GridStructerModel
    {
        public long PkGridId { get; set; }

        public long FkUserId { get; set; }

        public long FkFormId { get; set; }

        public string GridName { get; set; }

        public string JsonData { get; set; }
    }
}
