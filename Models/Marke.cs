using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServisWeb.Models
{
    [Table("Marke")]
    public class Marke
    {
        public Marke()
        {
            this.Vozilas = new HashSet<Vozila>();
        }

        [Key]
        public int MarkaID { get; set; }
        public string NazivMarke { get; set; }

        public virtual ICollection<Vozila> Vozilas { get; set; }
    }
}