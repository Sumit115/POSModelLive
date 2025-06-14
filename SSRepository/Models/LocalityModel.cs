﻿using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class LocalityModel : BaseModel
    {
        public long PKID { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(50)] 
        public string LocalityName { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select Area")]  
        public long FkAreaId { get; set; }
        public string? Description { get; set; }
        public string? AreaName { get; set; }
    }

}
