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

        public ActionResult Quick(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return RedirectToAction("Index", "Home");

            var vozila = db.Vozilas
                .Include(v => v.Marke)
                .Where(v => v.RegistarskaOznaka.Contains(q)
                         || v.Model.Contains(q)
                         || v.Marke.NazivMarke.Contains(q)
                         || v.BrojSasije.Contains(q))
                .ToList();

            var servisi = db.Servisis
                .Include(s => s.Vozila)
                .Include(s => s.KategorijeServisa)
                .Where(s => s.OpisRadova.Contains(q)
                         || s.Vozila.RegistarskaOznaka.Contains(q))
                .ToList();

            ViewBag.Query = q;
            ViewBag.Vozila = vozila;
            ViewBag.Servisi = servisi;
            return View();
        }

        public ActionResult Index(SearchViewModel model)
        {
            model.Marke = db.Markes
                .Select(m => new System.Web.Mvc.SelectListItem
                {
                    Value = m.MarkaID.ToString(),
                    Text = m.NazivMarke
                }).ToList();

            model.Kategorije = db.KategorijeServisas
                .Select(k => new System.Web.Mvc.SelectListItem
                {
                    Value = k.KategorijaID.ToString(),
                    Text = k.NazivKategorije
                }).ToList();

            model.PretrazivanjeIzvrseno = true;

            var servisi = db.Servisis
                .Include(s => s.Vozila.Marke)
                .Include(s => s.Vozila.Korisnici)
                .Include(s => s.KategorijeServisa)
                .Include(s => s.Mehanicari)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.KljucnaRec))
                servisi = servisi.Where(s =>
                    s.Vozila.RegistarskaOznaka.Contains(model.KljucnaRec) ||
                    s.Vozila.Model.Contains(model.KljucnaRec) ||
                    s.Vozila.Marke.NazivMarke.Contains(model.KljucnaRec) ||
                    s.Vozila.BrojSasije.Contains(model.KljucnaRec) ||
                    s.OpisRadova.Contains(model.KljucnaRec));

            if (model.MarkaID.HasValue)
                servisi = servisi.Where(s => s.Vozila.MarkaID == model.MarkaID);

            if (model.GodinaOd.HasValue)
                servisi = servisi.Where(s => s.Vozila.GodinaProizvodnje >= model.GodinaOd);

            if (model.GodinaDo.HasValue)
                servisi = servisi.Where(s => s.Vozila.GodinaProizvodnje <= model.GodinaDo);

            if (model.KategorijaID.HasValue)
                servisi = servisi.Where(s => s.KategorijaID == model.KategorijaID);

            if (model.CenaOd.HasValue)
                servisi = servisi.Where(s => s.UkupnaCena >= model.CenaOd);

            if (model.CenaDo.HasValue)
                servisi = servisi.Where(s => s.UkupnaCena <= model.CenaDo);

            model.ServisRezultati = servisi.OrderByDescending(s => s.DatumServisa).ToList();

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}