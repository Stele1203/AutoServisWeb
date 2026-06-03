using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServisWeb.Models
{
    [Table("Delovi")]
    public class Delovi
    {
        public Delovi()
        {
            this.ServisStavkes = new HashSet<ServisStavke>();
        }

        [Key]
        public int DeoID { get; set; }
        public string NazivDela { get; set; }
        public string Proizvodjac { get; set; }
        public decimal Cena { get; set; }

        public virtual ICollection<ServisStavke> ServisStavkes { get; set; }
    }
}