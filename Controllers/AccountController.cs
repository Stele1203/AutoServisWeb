using System.Linq;
using System.Web.Mvc;
using AutoServisWeb.Models;
using AutoServisWeb.ViewModels;

namespace AutoServisWeb.Controllers
{
    public class AccountController : Controller
    {
        private AutoDnevnik_DBEntities db = new AutoDnevnik_DBEntities();

        public ActionResult Login(string returnUrl)
        {
            if (Session["KorisnikID"] != null)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var korisnik = db.Korisnicis
                .FirstOrDefault(k => k.Email == model.Email && k.Lozinka == model.Lozinka);

            if (korisnik == null)
            {
                ModelState.AddModelError("", "Pogresna email adresa ili lozinka.");
                return View(model);
            }

            Session["KorisnikID"] = korisnik.KorisnikID;
            Session["UlogaID"] = korisnik.UlogaID ?? 2;
            Session["Ime"] = korisnik.Ime + " " + korisnik.Prezime;

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            if (Session["KorisnikID"] != null)
                return RedirectToAction("Index", "Home");

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (db.Korisnicis.Any(k => k.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Korisnik sa ovim emailom vec postoji.");
                return View(model);
            }

            var korisnik = new Korisnici
            {
                Ime = model.Ime,
                Prezime = model.Prezime,
                Email = model.Email,
                Lozinka = model.Lozinka,
                UlogaID = 2
            };

            db.Korisnicis.Add(korisnik);
            db.SaveChanges();

            Session["KorisnikID"] = korisnik.KorisnikID;
            Session["UlogaID"] = 2;
            Session["Ime"] = korisnik.Ime + " " + korisnik.Prezime;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}