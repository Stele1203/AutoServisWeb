using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoServisWeb.Filters;
using AutoServisWeb.Models;

namespace AutoServisWeb.Controllers
{
    public class MehanicariController : Controller
    {
        private AutoDnevnik_DBEntities db = new AutoDnevnik_DBEntities();

        public ActionResult Index()
        {
            return View(db.Mehanicaris.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var m = db.Mehanicaris
                .Include(x => x.Servisis)
                .FirstOrDefault(x => x.MehanicarID == id);
            if (m == null) return HttpNotFound();
            return View(m);
        }

        [AdminOnly]
        public ActionResult Create()
        {
            return View();
        }

        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MehanicarID,ImePrezime,Telefon,AdresaRadionice")] Mehanicari m)
        {
            if (ModelState.IsValid)
            {
                db.Mehanicaris.Add(m);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(m);
        }

        [AdminOnly]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var m = db.Mehanicaris.Find(id);
            if (m == null) return HttpNotFound();
            return View(m);
        }

        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MehanicarID,ImePrezime,Telefon,AdresaRadionice")] Mehanicari m)
        {
            if (ModelState.IsValid)
            {
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(m);
        }

        [AdminOnly]
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var m = db.Mehanicaris.Find(id);
            if (m == null) return HttpNotFound();
            return View(m);
        }

        [AdminOnly]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var m = db.Mehanicaris.Find(id);
            db.Mehanicaris.Remove(m);
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