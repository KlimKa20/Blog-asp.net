using System.ComponentModel.DataAnnotations;

namespace WebApplication4.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
