using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServisWeb.Models
{
    [Table("LogBrisanihServisa")]
    public class LogBrisanihServisa
    {
        [Key]
        public int LogID { get; set; }
        public int? ServisID { get; set; }
        public DateTime? DatumBrisanja { get; set; }
    }
}