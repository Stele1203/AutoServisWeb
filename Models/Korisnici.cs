using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServisWeb.Models
{
    [Table("Korisnici")]
    public class Korisnici
    {
        public Korisnici()
        {
            this.Vozilas = new HashSet<Vozila>();
        }

        [Key]
        public int KorisnikID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; }
        public int? UlogaID { get; set; }

        public virtual Uloge Uloge { get; set; }
        public virtual ICollection<Vozila> Vozilas { get; set; }
    }
}