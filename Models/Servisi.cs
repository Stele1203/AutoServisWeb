using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServisWeb.Models
{
    [Table("Servisi")]
    public class Servisi
    {
        public Servisi()
        {
            this.ServisStavkes = new HashSet<ServisStavke>();
            this.Komentaris = new HashSet<Komentari>();
        }

        [Key]
        public int ServisID { get; set; }
        public int? VoziloID { get; set; }
        public int? MehanicarID { get; set; }
        public int? KategorijaID { get; set; }
        public DateTime DatumServisa { get; set; }
        public int Kilometraza { get; set; }
        public string OpisRadova { get; set; }
        public decimal UkupnaCena { get; set; }

        public virtual Vozila Vozila { get; set; }
        public virtual Mehanicari Mehanicari { get; set; }
        public virtual KategorijeServisa KategorijeServisa { get; set; }
        public virtual ICollection<ServisStavke> ServisStavkes { get; set; }
        public virtual ICollection<Komentari> Komentaris { get; set; }
    }
}