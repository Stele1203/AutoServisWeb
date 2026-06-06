using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoServisWeb.Models;
using AutoServisWeb.ViewModels;

namespace AutoServisWeb.Controllers
{
    public class SearchController : Controller
    {
        private AutoDnevnik_DBEntities db = new AutoDnevnik_DBEntities();

        // Brza pretraga — navbar search box
        public ActionResult Quick(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return RedirectToAction("Index", "Home");

            var vozila = db.Vozilas
                .Include(v => v.Marke)
                .Where(v => v.RegistarskaOznaka.Contains(q)
                         || v.Model.Contains(q)
                         || v.Marke.NazivMarke.Contains(q))
                .ToList();

            var servisi = db.Servisis
                .Include(s => s.Vozila)
                .Include(s => s.KategorijeServisa)
                .Include(s => s.Mehanicari)
                .Where(s => s.OpisRadova.Contains(q)
                         || s.Vozila.RegistarskaOznaka.Contains(q)
                         || s.Mehanicari.ImePrezime.Contains(q))
                .ToList();

            var mehanicari = db.Mehanicaris
                .Where(m => m.ImePrezime.Contains(q))
                .ToList();

            ViewBag.Query = q;
            ViewBag.Vozila = vozila;
            ViewBag.Servisi = servisi;
            ViewBag.Mehanicari = mehanicari;
            return View();
        }

        // Detaljna pretraga
        public ActionResult Index(SearchViewModel model)
        {
            model.Marke = db.Markes
                .Select(m => new SelectListItem { Value = m.MarkaID.ToString(), Text = m.NazivMarke })
                .ToList();

            model.Kategorije = db.KategorijeServisas
                .Select(k => new SelectListItem { Value = k.KategorijaID.ToString(), Text = k.NazivKategorije })
                .ToList();

            if (!string.IsNullOrWhiteSpace(model.KljucnaRec) || model.MarkaID.HasValue
                || model.GodinaOd.HasValue || model.GodinaDo.HasValue
                || model.KategorijaID.HasValue || model.CenaOd.HasValue || model.CenaDo.HasValue)
            {
                model.PretrazivanjeIzvrseno = true;

                var vozila = db.Vozilas.Include(v => v.Marke).Include(v => v.Korisnici).AsQueryable();

                if (!string.IsNullOrWhiteSpace(model.KljucnaRec))
                    vozila = vozila.Where(v => v.Model.Contains(model.KljucnaRec)
                                           || v.RegistarskaOznaka.Contains(model.KljucnaRec)
                                           || v.Marke.NazivMarke.Contains(model.KljucnaRec));

                if (model.MarkaID.HasValue)
                    vozila = vozila.Where(v => v.MarkaID == model.MarkaID);

                if (model.GodinaOd.HasValue)
                    vozila = vozila.Where(v => v.GodinaProizvodnje >= model.GodinaOd);

                if (model.GodinaDo.HasValue)
                    vozila = vozila.Where(v => v.GodinaProizvodnje <= model.GodinaDo);

                model.VozilaRezultati = vozila.ToList();

                var servisi = db.Servisis
                    .Include(s => s.Vozila.Marke)
                    .Include(s => s.KategorijeServisa)
                    .Include(s => s.Mehanicari)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(model.KljucnaRec))
                    servisi = servisi.Where(s => s.OpisRadova.Contains(model.KljucnaRec));

                if (model.KategorijaID.HasValue)
                    servisi = servisi.Where(s => s.KategorijaID == model.KategorijaID);

                if (model.CenaOd.HasValue)
                    servisi = servisi.Where(s => s.UkupnaCena >= model.CenaOd);

                if (model.CenaDo.HasValue)
                    servisi = servisi.Where(s => s.UkupnaCena <= model.CenaDo);

                model.ServisRezultati = servisi.ToList();
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}