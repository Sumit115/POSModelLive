﻿using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
     public class CityModel : BaseModel
    {
        public long PKID { get; set; }
        [Required]
        public string CityName { get; set; }

        [Required]
        public string StateName { get; set; }

    }
}
