using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Services.Description;

namespace AutoServisWeb.Models
{
    [Table("KategorijeServisa")]
    public class KategorijeServisa
    {
        public KategorijeServisa()
        {
            this.Servisis = new HashSet<Servisi>();
        }

        [Key]
        public int KategorijaID { get; set; }
        public string NazivKategorije { get; set; }

        public virtual ICollection<Servisi> Servisis { get; set; }
    }
}