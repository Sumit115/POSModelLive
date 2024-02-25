using System.ComponentModel.DataAnnotations;

namespace SSRepository.Models
{
    public class SignInModel
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
