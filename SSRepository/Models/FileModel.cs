using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class   FileModel
    {
        [Required(ErrorMessage = "Please select Series")] 
        public long FKSeriesId { get; set; }
        public string? SeriesName { get; set; }

        [Required(ErrorMessage = "Please select file")]
        public IFormFile File { get; set; }
    }
    public class ImportFileModel
    {
        
        [Required(ErrorMessage = "Please select file")]
        public IFormFile File { get; set; }
    }
}
