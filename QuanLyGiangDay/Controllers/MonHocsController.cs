using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyGiangDay.Models.EF;

namespace QuanLyGiangDay.Controllers
{
    public class MonHocsController : Controller
    {
        private quanlygiaovienEntities db = new quanlygiaovienEntities();

        // GET: MonHocs
        public ActionResult Index()
        {
            return View(db.MonHoc.ToList());
        }

        // GET: MonHocs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHoc.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            return View(monHoc);
        }

        public ActionResult _PartialDetail(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHoc.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialDetail", monHoc);
        }

        // GET: MonHocs/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult _PartialCreate()
        {
            return PartialView("_PartialCreate");
        }

        // POST: MonHocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaMH,TenMon,MoTa,SoGio")] MonHoc monHoc)
        {
            if (ModelState.IsValid)
            {
                db.MonHoc.Add(monHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(monHoc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialCreate([Bind(Include = "TenMon,MoTa,SoGio")] MonHoc monHoc)
        {
            if (ModelState.IsValid)
            {
                var countOfRows = db.MonHoc.Count();
                var lastRow = db.MonHoc.OrderBy(c => c.MaMH).Skip(countOfRows - 1).FirstOrDefault();
                int nextId = Convert.ToInt32(lastRow.MaMH.Substring(2));
                monHoc.MaMH = "MH" + (nextId + 1);
                db.MonHoc.Add(monHoc);
                db.SaveChanges();
                return Json(new { Success = true });
            }

            return Json(new { Success = false, Message = "Lỗi! Không thể thêm mới" });
        }

        // GET: MonHocs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHoc.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            return View(monHoc);
        }

        public ActionResult _PartialEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHoc.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialEdit", monHoc);
        }

        // POST: MonHocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaMH,TenMon,MoTa,SoGio")] MonHoc monHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(monHoc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialEdit([Bind(Include = "MaMH,TenMon,MoTa,SoGio")] MonHoc monHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monHoc).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Success = true });
            }
            return Json(new { Success = false, Message = "Lỗi! Không thể cập nhật môn học này" });
        }

        // GET: MonHocs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHoc.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            return View(monHoc);
        }

        public ActionResult _PartialDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHoc.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialDelete", monHoc);
        }

        // POST: MonHocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MonHoc monHoc = db.MonHoc.Find(id);
            db.MonHoc.Remove(monHoc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("_PartialDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialDeleteConfirmed(string id)
        {
            MonHoc monHoc = db.MonHoc.Find(id);
            db.MonHoc.Remove(monHoc);
            db.SaveChanges();
            return Json(new { Success = true });
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
