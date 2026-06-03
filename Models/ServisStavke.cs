using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServisWeb.Models
{
    [Table("ServisStavke")]
    public class ServisStavke
    {
        [Key]
        public int StavkaID { get; set; }
        public int? ServisID { get; set; }
        public int? DeoID { get; set; }
        public int Kolicina { get; set; }
        public decimal? UkupnoStavka { get; set; }

        public virtual Servisi Servisi { get; set; }
        public virtual Delovi Delovi { get; set; }
    }
}