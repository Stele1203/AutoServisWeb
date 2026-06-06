using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoServisWeb.Filters;
using AutoServisWeb.Models;
using AutoServisWeb.ViewModels;

namespace AutoServisWeb.Controllers
{
    [AdminOnly]
    public class AdminController : Controller
    {
        private AutoDnevnik_DBEntities db = new AutoDnevnik_DBEntities();

        public ActionResult Korisnici()
        {
            var korisnici = db.Korisnicis.Include(k => k.Uloge).ToList();
            return View(korisnici);
        }

        public ActionResult EditKorisnik(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var k = db.Korisnicis.Find(id);
            if (k == null) return HttpNotFound();
            ViewBag.UlogaID = new SelectList(db.Uloges, "UlogaID", "NazivUloge", k.UlogaID);
            return View(k);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKorisnik([Bind(Include = "KorisnikID,Ime,Prezime,Email,UlogaID")] Korisnici k)
        {
            if (ModelState.IsValid)
            {
                var existing = db.Korisnicis.Find(k.KorisnikID);
                existing.Ime = k.Ime;
                existing.Prezime = k.Prezime;
                existing.Email = k.Email;
                existing.UlogaID = k.UlogaID;
                db.SaveChanges();
                return RedirectToAction("Korisnici");
            }
            ViewBag.UlogaID = new SelectList(db.Uloges, "UlogaID", "NazivUloge", k.UlogaID);
            return View(k);
        }

        public ActionResult ChangePassword(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var k = db.Korisnicis.Find(id);
            if (k == null) return HttpNotFound();
            ViewBag.ImePrezime = k.Ime + " " + k.Prezime;
            return View(new ChangePasswordViewModel { KorisnikID = id.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var k = db.Korisnicis.Find(model.KorisnikID);
                ViewBag.ImePrezime = k?.Ime + " " + k?.Prezime;
                return View(model);
            }
            var korisnik = db.Korisnicis.Find(model.KorisnikID);
            if (korisnik == null) return HttpNotFound();
            korisnik.Lozinka = model.NovaLozinka;
            db.SaveChanges();
            TempData["Uspjeh"] = "Lozinka je uspjesno promijenjena.";
            return RedirectToAction("Korisnici");
        }

        public ActionResult DeleteKorisnik(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var k = db.Korisnicis.Include(x => x.Uloge).FirstOrDefault(x => x.KorisnikID == id);
            if (k == null) return HttpNotFound();
            return View(k);
        }

        [HttpPost, ActionName("DeleteKorisnik")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteKorisnikConfirmed(int id)
        {
            if ((int)Session["KorisnikID"] == id)
            {
                TempData["Greska"] = "Ne mozete obrisati vlastiti nalog.";
                return RedirectToAction("Korisnici");
            }
            var k = db.Korisnicis.Find(id);

            var komentari = db.Komentaris.Where(c => c.KorisnikID == id).ToList();
            db.Komentaris.RemoveRange(komentari);

            db.Korisnicis.Remove(k);
            db.SaveChanges();
            return RedirectToAction("Korisnici");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}