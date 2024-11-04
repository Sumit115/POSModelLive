using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientRoleRightDtl
    {
        public long PkroleRightId { get; set; }
        public long FkroleId { get; set; }
        public long FkformId { get; set; }
        public bool RightNoAccess { get; set; }
        public bool RightAdd { get; set; }
        public bool RightEdit { get; set; }
        public bool RightDelete { get; set; }
        public bool RightDraft { get; set; }
        public bool RightPrint { get; set; }
        public bool RightView { get; set; }
        public bool RightBrowse { get; set; }
        public bool RightCancel { get; set; }
        public bool RightLockEdit { get; set; }
        public bool RightLockDelete { get; set; }
        public bool RightLockPrint { get; set; }
    }
}
