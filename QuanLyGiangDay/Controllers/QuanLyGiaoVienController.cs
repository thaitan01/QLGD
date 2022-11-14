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
    public class QuanLyGiaoVienController : Controller
    {
        private quanlygiaovienEntities db = new quanlygiaovienEntities();

        // GET: QuanLyGiaoVien
        public ActionResult Index()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.taikhoan = Session["taikhoan"];
            var giaoViens = db.GiaoViens.Include(g => g.LoaiGV);
            return View(giaoViens.ToList());
        }

        // GET: QuanLyGiaoVien/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiaoVien giaoVien = db.GiaoViens.Find(id);
            if (giaoVien == null)
            {
                return HttpNotFound();
            }
            return View(giaoVien);
        }
        private string rendumID()
        {
            String id = "GV";
            id += ((from count in db.GiaoViens select count).Count()+1).ToString();
            return id;
        }
        // GET: QuanLyGiaoVien/Create
        public ActionResult Create()
        {
            ViewBag.id = rendumID();
            ViewBag.MaLoaiGV = new SelectList(db.LoaiGVs, "MaLoaiGV", "TenLoaiGV");
            return View();
        }

        // POST: QuanLyGiaoVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaGV,TenGV,MaLoaiGV,Email,Sdt")] GiaoVien giaoVien)
        {
            if((from count in db.GiaoViens where count.Email == giaoVien.Email select count).Count() > 0)
            {
                ViewBag.Erro ="Email\t[" + giaoVien.Email + "]\tđã tồn tại, vui lòng không nhập trùng email.";
                var giaoViens = db.GiaoViens.Include(g => g.LoaiGV);
                return View("Index", giaoViens);
            }
            if (ModelState.IsValid)
            {
                db.GiaoViens.Add(giaoVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiGV = new SelectList(db.LoaiGVs, "MaLoaiGV", "TenLoaiGV", giaoVien.MaLoaiGV);
            return View(giaoVien);
        }

        // GET: QuanLyGiaoVien/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiaoVien giaoVien = db.GiaoViens.Find(id);
            if (giaoVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiGV = new SelectList(db.LoaiGVs, "MaLoaiGV", "TenLoaiGV", giaoVien.MaLoaiGV);
            return View(giaoVien);
        }

        // POST: QuanLyGiaoVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaGV,TenGV,MaLoaiGV,Email,Sdt")] GiaoVien giaoVien)
        {
            if (
                (
                         from count in db.GiaoViens 
                         where 
                          (count.Email.Trim() == giaoVien.Email.Trim())  
                          && (count.MaGV.Trim() != giaoVien.MaGV)
                         select count
                     ).Count() > 0
                )
            {
                ViewBag.Erro = "Email\t[" + giaoVien.Email.Trim() + "]\tđã tồn tại, vui lòng không nhập trùng email.";
                var giaoViens = db.GiaoViens.Include(g => g.LoaiGV);
                return View("Index", giaoViens);
            }
            if (ModelState.IsValid)
            {
                db.Entry(giaoVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiGV = new SelectList(db.LoaiGVs, "MaLoaiGV", "TenLoaiGV", giaoVien.MaLoaiGV);
            return View(giaoVien);
        }

        // GET: QuanLyGiaoVien/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiaoVien giaoVien = db.GiaoViens.Find(id);
            if (giaoVien == null)
            {
                return HttpNotFound();
            }
            return View(giaoVien);
        }

        // POST: QuanLyGiaoVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GiaoVien giaoVien = db.GiaoViens.Find(id);
            db.GiaoViens.Remove(giaoVien);
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
