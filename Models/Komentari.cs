using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServisWeb.Models
{
    [Table("Komentari")]
    public class Komentari
    {
        [Key]
        public int KomentarID { get; set; }
        public int? ServisID { get; set; }
        public int? KorisnikID { get; set; }
        public string Tekst { get; set; }
        public DateTime DatumKomentara { get; set; }

        public virtual Servisi Servisi { get; set; }
        public virtual Korisnici Korisnici { get; set; }
    }
}