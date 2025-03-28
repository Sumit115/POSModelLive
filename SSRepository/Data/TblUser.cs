﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblUser_mas", Schema = "dbo")]
    public partial class TblUserMas : TblBase, IEntity
    {
        public TblUserMas()
        {
            LocationUsers = new HashSet<TblUserLocLnk>();
        }
        [Key]
        public long PkUserId { get; set; }  
        public string UserId { get; set; } = null!;

        public string Pwd { get; set; } = null!;

        public long? FkRegId { get; set; }

        public int? Usertype { get; set; }

        public long? FkBranchId { get; set; }

        public long FkRoleId { get; set; }

        public DateTime? Expiredt { get; set; }

        public DateTime? ExpirePwddt { get; set; }

        public long FkEmployeeId { get; set; }

        public int IsAdmin { get; set; }

        public virtual TblEmployeeMas? FkEmployee { get; set; }
        public virtual TblBranchMas? FkBranch { get; set; }
        public virtual TblRoleMas? FkRole { get; set; }
        public virtual ICollection<TblUserLocLnk> LocationUsers { get; set; }

    }
}
