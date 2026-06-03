using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServisWeb.Models
{
    [Table("Registracije")]
    public class Registracije
    {
        [Key]
        public int RegistracijaID { get; set; }
        public int? VoziloID { get; set; }
        public DateTime DatumIsteka { get; set; }
        public decimal CenaRegistracije { get; set; }

        public virtual Vozila Vozila { get; set; }
    }
}