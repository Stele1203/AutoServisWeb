using System.ComponentModel.DataAnnotations;

namespace AutoServisWeb.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress(ErrorMessage = "Neispravan format emaila")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }

        public string ReturnUrl { get; set; }
    }
}