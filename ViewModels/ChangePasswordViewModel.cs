using System.ComponentModel.DataAnnotations;

namespace AutoServisWeb.ViewModels
{
    public class ChangePasswordViewModel
    {
        public int KorisnikID { get; set; }

        [Required(ErrorMessage = "Nova lozinka je obavezna")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lozinka mora imati najmanje 6 karaktera")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova lozinka")]
        public string NovaLozinka { get; set; }

        [Required(ErrorMessage = "Potvrdite lozinku")]
        [DataType(DataType.Password)]
        [Compare("NovaLozinka", ErrorMessage = "Lozinke se ne podudaraju")]
        [Display(Name = "Potvrda lozinke")]
        public string PotvrdaLozinke { get; set; }
    }
}