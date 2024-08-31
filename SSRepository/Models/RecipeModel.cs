using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class RecipeModel : BaseModel
    {
        public long PkRecipeId { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(50)] 
        public string Name { get; set; }
        
        public List<RecipeDtlModel>? Recipe_dtl { get; set; }

    }
    public partial class RecipeDtlModel : BaseModel
    {
        public long FkRecipeId { get; set; }
        public long SrNo { get; set; }
        public string TranType { get; set; }//=I=IN,O=OUT 
        public long FkProductId { get; set; }
        public string? Batch { get; set; }
        public string? Color { get; set; } //Only Purchase
        public decimal Qty { get; set; }//txt
        public string? Product { get; set; }//txt
    }

}
