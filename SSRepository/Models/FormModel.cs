﻿using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
     public class FormModel  
    {
        public long PKID { get; set; }
        public string FormName { get; set; }
        public string? MasterForm { get; set; }
        public int SeqNo { get; set; }

        public long? FKMasterFormID { get; set; }
        public string? ShortName { get; set; }
        public string? ShortCut { get; set; }
        public string? ToolTip { get; set; }
        public string Image { get; set; }
        public char? FormType { get; set; }
        public string? WebURL { get; set; }
        public bool IsActive { get; set; }
         
    }
}
