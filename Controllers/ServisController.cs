using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoServisWeb.Filters;
using AutoServisWeb.Models;

namespace AutoServisWeb.Controllers
{
    public class ServisController : Controller
    {
        private AutoDnevnik_DBEntities db = new AutoDnevnik_DBEntities();

        public ActionResult Index()
        {
            var servisi = db.Servisis
                .Include(s => s.Vozila)
                .Include(s => s.Mehanicari)
                .Include(s => s.KategorijeServisa)
                .OrderByDescending(s => s.DatumServisa)
                .ToList();
            return View(servisi);
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var servis = db.Servisis
                .Include(s => s.Vozila.Marke)
                .Include(s => s.Mehanicari)
                .Include(s => s.KategorijeServisa)
                .Include(s => s.ServisStavkes.Select(ss => ss.Delovi))
                .Include(s => s.Komentaris.Select(k => k.Korisnici))
                .FirstOrDefault(s => s.ServisID == id);
            if (servis == null) return HttpNotFound();
            return View(servis);
        }

        [AdminOnly]
        public ActionResult Create()
        {
            ViewBag.VoziloID = new SelectList(db.Vozilas, "VoziloID", "RegistarskaOznaka");
            ViewBag.MehanicarID = new SelectList(db.Mehanicaris, "MehanicarID", "ImePrezime");
            ViewBag.KategorijaID = new SelectList(db.KategorijeServisas, "KategorijaID", "NazivKategorije");
            return View();
        }

        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServisID,VoziloID,MehanicarID,KategorijaID,DatumServisa,Kilometraza,OpisRadova,UkupnaCena")] Servisi servis)
        {
            if (ModelState.IsValid)
            {
                db.Servisis.Add(servis);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VoziloID = new SelectList(db.Vozilas, "VoziloID", "RegistarskaOznaka", servis.VoziloID);
            ViewBag.MehanicarID = new SelectList(db.Mehanicaris, "MehanicarID", "ImePrezime", servis.MehanicarID);
            ViewBag.KategorijaID = new SelectList(db.KategorijeServisas, "KategorijaID", "NazivKategorije", servis.KategorijaID);
            return View(servis);
        }

        [AdminOnly]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var servis = db.Servisis.Find(id);
            if (servis == null) return HttpNotFound();
            ViewBag.VoziloID = new SelectList(db.Vozilas, "VoziloID", "RegistarskaOznaka", servis.VoziloID);
            ViewBag.MehanicarID = new SelectList(db.Mehanicaris, "MehanicarID", "ImePrezime", servis.MehanicarID);
            ViewBag.KategorijaID = new SelectList(db.KategorijeServisas, "KategorijaID", "NazivKategorije", servis.KategorijaID);
            return View(servis);
        }

        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServisID,VoziloID,MehanicarID,KategorijaID,DatumServisa,Kilometraza,OpisRadova,UkupnaCena")] Servisi servis)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VoziloID = new SelectList(db.Vozilas, "VoziloID", "RegistarskaOznaka", servis.VoziloID);
            ViewBag.MehanicarID = new SelectList(db.Mehanicaris, "MehanicarID", "ImePrezime", servis.MehanicarID);
            ViewBag.KategorijaID = new SelectList(db.KategorijeServisas, "KategorijaID", "NazivKategorije", servis.KategorijaID);
            return View(servis);
        }

        [AdminOnly]
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var servis = db.Servisis
                .Include(s => s.Vozila)
                .Include(s => s.Mehanicari)
                .Include(s => s.KategorijeServisa)
                .FirstOrDefault(s => s.ServisID == id);
            if (servis == null) return HttpNotFound();
            return View(servis);
        }

        [AdminOnly]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var servis = db.Servisis.Find(id);
            db.Servisis.Remove(servis);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [LoginRequired]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajKomentar(int servisId, string tekst)
        {
            if (!string.IsNullOrWhiteSpace(tekst))
            {
                var komentar = new Komentari
                {
                    ServisID = servisId,
                    KorisnikID = (int)Session["KorisnikID"],
                    Tekst = tekst,
                    DatumKomentara = DateTime.Now
                };
                db.Komentaris.Add(komentar);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = servisId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}