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
    public class HocKyController : Controller
    {
        private quanlygiaovienEntities db = new quanlygiaovienEntities();

        // GET: HocKy
        public ActionResult Index()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.taikhoan = Session["taikhoan"];
            return View(db.HocKies.ToList());
        }

        // GET: HocKy/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocKy hocKy = db.HocKies.Find(id);
            if (hocKy == null)
            {
                return HttpNotFound();
            }
            return View(hocKy);
        }

        //
        private string rendumID()
        {
            String id = "HK";
            id += ((from count in db.HocKies select count).Count()+1).ToString();
            return id;
        }

        // GET: HocKy/Create
        public ActionResult Create()
        {
            ViewBag.id = rendumID();
            return View();
        }

        // POST: HocKy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHK,TenHK")] HocKy hocKy)
        {
            if (ModelState.IsValid)
            {
                db.HocKies.Add(hocKy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hocKy);
        }

        // GET: HocKy/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocKy hocKy = db.HocKies.Find(id);
            if (hocKy == null)
            {
                return HttpNotFound();
            }
            return View(hocKy);
        }

        // POST: HocKy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHK,TenHK")] HocKy hocKy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hocKy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hocKy);
        }

        // GET: HocKy/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocKy hocKy = db.HocKies.Find(id);
            if (hocKy == null)
            {
                return HttpNotFound();
            }
            return View(hocKy);
        }

        // POST: HocKy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HocKy hocKy = db.HocKies.Find(id);
            db.HocKies.Remove(hocKy);
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