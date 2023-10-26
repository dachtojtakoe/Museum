using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExcursionsCompany.Models;

namespace ExcursionsCompany.Views
{
    public class ExcursionTypesController : Controller
    {
        private MuseumDataEntities db = new MuseumDataEntities();

        // GET: ExcursionTypes
        public ActionResult Index()
        {
            return View(db.ExcursionTypes.ToList());
        }

        // GET: ExcursionTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExcursionTypes excursionTypes = db.ExcursionTypes.Find(id);
            if (excursionTypes == null)
            {
                return HttpNotFound();
            }
            return View(excursionTypes);
        }

        // GET: ExcursionTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExcursionTypes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,duration,description,deleted,Image")] ExcursionTypes excursionTypes)
        {
            if (ModelState.IsValid)
            {
                db.ExcursionTypes.Add(excursionTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(excursionTypes);
        }

        // GET: ExcursionTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExcursionTypes excursionTypes = db.ExcursionTypes.Find(id);
            if (excursionTypes == null)
            {
                return HttpNotFound();
            }
            return View(excursionTypes);
        }

        // POST: ExcursionTypes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,duration,description,deleted,Image")] ExcursionTypes excursionTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(excursionTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(excursionTypes);
        }

        // GET: ExcursionTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExcursionTypes excursionTypes = db.ExcursionTypes.Find(id);
            if (excursionTypes == null)
            {
                return HttpNotFound();
            }
            return View(excursionTypes);
        }

        // POST: ExcursionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExcursionTypes excursionTypes = db.ExcursionTypes.Find(id);
            db.ExcursionTypes.Remove(excursionTypes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
