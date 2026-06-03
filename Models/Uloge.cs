using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServisWeb.Models
{
    [Table("Uloge")]
    public class Uloge
    {
        public Uloge()
        {
            this.Korisnicis = new HashSet<Korisnici>();
        }

        [Key]
        public int UlogaID { get; set; }
        public string NazivUloge { get; set; }

        public virtual ICollection<Korisnici> Korisnicis { get; set; }
    }
}