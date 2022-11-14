using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyGiangDay.Models.EF;

namespace QuanLyGiangDay.Controllers
{
    public class CTDTController : Controller
    {
        private quanlygiaovienEntities db = new quanlygiaovienEntities();

        // GET: CTDT
        public ActionResult Index()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.taikhoan = Session["taikhoan"];
            return View(db.CTDTs.ToList());
        }

        // GET: CTDT/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDT cTDT = db.CTDTs.Find(id);
            if (cTDT == null)
            {
                return HttpNotFound();
            }
            return View(cTDT);
        }

        private string rendumID()
        {
            String id = "DT";
            id += ((from count in db.CTDTs select count).Count()+1).ToString();
            return id;
        }
        // GET: CTDT/Create
        public ActionResult Create()
        {
            ViewBag.id = rendumID();
            return View();
        }

        // POST: CTDT/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaCTDT,TenCTDT,MoTa")] CTDT cTDT)
        {
            if (ModelState.IsValid)
            {
                db.CTDTs.Add(cTDT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cTDT);
        }

        // GET: CTDT/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDT cTDT = db.CTDTs.Find(id);
            if (cTDT == null)
            {
                return HttpNotFound();
            }
            return View(cTDT);
        }

        // POST: CTDT/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaCTDT,TenCTDT,MoTa")] CTDT cTDT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTDT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cTDT);
        }

        // GET: CTDT/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDT cTDT = db.CTDTs.Find(id);
            if (cTDT == null)
            {
                return HttpNotFound();
            }
            return View(cTDT);
        }

        // POST: CTDT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CTDT cTDT = db.CTDTs.Find(id);
            db.CTDTs.Remove(cTDT);
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
