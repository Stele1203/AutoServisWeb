using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoServisWeb.Filters;
using AutoServisWeb.Models;

namespace AutoServisWeb.Controllers
{
    public class VozilaController : Controller
    {
        private AutoDnevnik_DBEntities db = new AutoDnevnik_DBEntities();

        public ActionResult Index()
        {
            var vozila = db.Vozilas
                .Include(v => v.Korisnici)
                .Include(v => v.Marke)
                .ToList();
            return View(vozila);
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vozilo = db.Vozilas
                .Include(v => v.Korisnici)
                .Include(v => v.Marke)
                .Include(v => v.Servisis)
                .Include(v => v.Registracijes)
                .FirstOrDefault(v => v.VoziloID == id);
            if (vozilo == null) return HttpNotFound();
            return View(vozilo);
        }

        [AdminOnly]
        public ActionResult Create()
        {
            ViewBag.KorisnikID = new SelectList(db.Korisnicis, "KorisnikID", "Email");
            ViewBag.MarkaID = new SelectList(db.Markes, "MarkaID", "NazivMarke");
            return View();
        }

        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VoziloID,KorisnikID,MarkaID,Model,GodinaProizvodnje,RegistarskaOznaka,BrojSasije")] Vozila vozilo)
        {
            if (ModelState.IsValid)
            {
                db.Vozilas.Add(vozilo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KorisnikID = new SelectList(db.Korisnicis, "KorisnikID", "Email", vozilo.KorisnikID);
            ViewBag.MarkaID = new SelectList(db.Markes, "MarkaID", "NazivMarke", vozilo.MarkaID);
            return View(vozilo);
        }

        [AdminOnly]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vozilo = db.Vozilas.Find(id);
            if (vozilo == null) return HttpNotFound();
            ViewBag.KorisnikID = new SelectList(db.Korisnicis, "KorisnikID", "Email", vozilo.KorisnikID);
            ViewBag.MarkaID = new SelectList(db.Markes, "MarkaID", "NazivMarke", vozilo.MarkaID);
            return View(vozilo);
        }

        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VoziloID,KorisnikID,MarkaID,Model,GodinaProizvodnje,RegistarskaOznaka,BrojSasije")] Vozila vozilo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vozilo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KorisnikID = new SelectList(db.Korisnicis, "KorisnikID", "Email", vozilo.KorisnikID);
            ViewBag.MarkaID = new SelectList(db.Markes, "MarkaID", "NazivMarke", vozilo.MarkaID);
            return View(vozilo);
        }

        [AdminOnly]
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vozilo = db.Vozilas
                .Include(v => v.Marke)
                .Include(v => v.Korisnici)
                .FirstOrDefault(v => v.VoziloID == id);
            if (vozilo == null) return HttpNotFound();
            return View(vozilo);
        }

        [AdminOnly]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var vozilo = db.Vozilas.Find(id);
            db.Vozilas.Remove(vozilo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}