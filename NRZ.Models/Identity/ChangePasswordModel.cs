using System.ComponentModel.DataAnnotations;

namespace NRZ.Models.Identity
{
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string NewPasswordConfirm { get; set; }

    }
}
