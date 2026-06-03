using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoServisWeb.Filters;
using AutoServisWeb.Models;

namespace AutoServisWeb.Controllers
{
    public class RegistracijeController : Controller
    {
        private AutoDnevnik_DBEntities db = new AutoDnevnik_DBEntities();

        public ActionResult Index()
        {
            var registracije = db.Registracijes
                .Include(r => r.Vozila)
                .Include(r => r.Vozila.Marke)
                .OrderByDescending(r => r.DatumIsteka)
                .ToList();
            return View(registracije);
        }

        [AdminOnly]
        public ActionResult Create()
        {
            ViewBag.VoziloID = new SelectList(db.Vozilas, "VoziloID", "RegistarskaOznaka");
            return View();
        }

        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RegistracijaID,VoziloID,DatumIsteka,CenaRegistracije")] Registracije reg)
        {
            if (ModelState.IsValid)
            {
                db.Registracijes.Add(reg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VoziloID = new SelectList(db.Vozilas, "VoziloID", "RegistarskaOznaka", reg.VoziloID);
            return View(reg);
        }

        [AdminOnly]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var reg = db.Registracijes.Find(id);
            if (reg == null) return HttpNotFound();
            ViewBag.VoziloID = new SelectList(db.Vozilas, "VoziloID", "RegistarskaOznaka", reg.VoziloID);
            return View(reg);
        }

        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RegistracijaID,VoziloID,DatumIsteka,CenaRegistracije")] Registracije reg)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VoziloID = new SelectList(db.Vozilas, "VoziloID", "RegistarskaOznaka", reg.VoziloID);
            return View(reg);
        }

        [AdminOnly]
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var reg = db.Registracijes
                .Include(r => r.Vozila)
                .FirstOrDefault(r => r.RegistracijaID == id);
            if (reg == null) return HttpNotFound();
            return View(reg);
        }

        [AdminOnly]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var reg = db.Registracijes.Find(id);
            db.Registracijes.Remove(reg);
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