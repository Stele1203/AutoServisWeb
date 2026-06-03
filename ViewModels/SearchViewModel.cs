using System.Collections.Generic;
using System.Web.Mvc;
using AutoServisWeb.Models;

namespace AutoServisWeb.ViewModels
{
    public class SearchViewModel
    {
        public string KljucnaRec { get; set; }
        public int? MarkaID { get; set; }
        public int? GodinaOd { get; set; }
        public int? GodinaDo { get; set; }
        public int? KategorijaID { get; set; }
        public decimal? CenaOd { get; set; }
        public decimal? CenaDo { get; set; }

        public List<Vozila> VozilaRezultati { get; set; }
        public List<Servisi> ServisRezultati { get; set; }

        public IEnumerable<SelectListItem> Marke { get; set; }
        public IEnumerable<SelectListItem> Kategorije { get; set; }

        public bool PretrazivanjeIzvrseno { get; set; }
    }
}