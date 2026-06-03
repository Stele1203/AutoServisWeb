using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Services.Description;

namespace AutoServisWeb.Models
{
    [Table("Mehanicari")]
    public class Mehanicari
    {
        public Mehanicari()
        {
            this.Servisis = new HashSet<Servisi>();
        }

        [Key]
        public int MehanicarID { get; set; }
        public string ImePrezime { get; set; }
        public string Telefon { get; set; }
        public string AdresaRadionice { get; set; }

        public virtual ICollection<Servisi> Servisis { get; set; }
    }
}