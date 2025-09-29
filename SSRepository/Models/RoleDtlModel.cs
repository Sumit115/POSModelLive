using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class RoleDtlModel : BaseModel
    {
        public long PkRoleDtlId { get; set; }

        public long FkRoleID { get; set; }

        public long FKFormID { get; set; }

        public Nullable<long> FKMasterFormID { get; set; }
        public int SeqNo { get; set; }
        public string? FormName { get; set; }
        public string? ShortName { get; set; }
        public string? ShortCut { get; set; }
        public string? ToolTip { get; set; }
        public string? Image { get; set; }
        public string? WebURL { get; set; }
        public bool IsActive { get; set; }
        public bool IsAccess { get; set; }
        public bool IsEdit { get; set; }
        public bool IsCreate { get; set; }
        public bool IsPrint { get; set; }
        public bool IsBrowse { get; set; }
        public bool IsDelete { get; set; }
        public List<RoleDtlModel>? SubMenu { get; set; }
    }

}
