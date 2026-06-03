using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Services.Description;

namespace AutoServisWeb.Models
{
    [Table("Vozila")]
    public class Vozila
    {
        public Vozila()
        {
            this.Servisis = new HashSet<Servisi>();
            this.Registracijes = new HashSet<Registracije>();
        }

        [Key]
        public int VoziloID { get; set; }
        public int? KorisnikID { get; set; }
        public int? MarkaID { get; set; }
        public string Model { get; set; }
        public int GodinaProizvodnje { get; set; }
        public string RegistarskaOznaka { get; set; }
        public string BrojSasije { get; set; }

        public virtual Korisnici Korisnici { get; set; }
        public virtual Marke Marke { get; set; }
        public virtual ICollection<Servisi> Servisis { get; set; }
        public virtual ICollection<Registracije> Registracijes { get; set; }
    }
}