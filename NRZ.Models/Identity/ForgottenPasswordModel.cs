using System.ComponentModel.DataAnnotations;

namespace NRZ.Models.Identity
{
    public class ForgottenPasswordModel
    {
        [Required]
        public string Token { get; set; }
        public string Email { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string NewPasswordConfirm { get; set; }
    }
}
