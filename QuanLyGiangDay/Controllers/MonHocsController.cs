using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
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
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.taikhoan = Session["taikhoan"];

            return View(db.MonHocs.ToList());
        }

        // GET: MonHocs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHocs.Find(id);
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
            MonHoc monHoc = db.MonHocs.Find(id);
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
            ViewBag.MaCTDT = new SelectList(db.CTDTs, "MaCTDT", "TenCTDT");
            ViewBag.MaHK = new SelectList(db.HocKies, "MaHK", "TenHK");
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
                db.MonHocs.Add(monHoc);
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
                var countOfRows = db.MonHocs.Count();
                var lastRow = db.MonHocs.OrderBy(c => c.MaMH).Skip(countOfRows - 1).FirstOrDefault();
                int nextId = Convert.ToInt32(lastRow.MaMH.Substring(2));
                
                monHoc.MaMH = "MH" + (nextId + 1);
                while (db.MonHocs.Find(monHoc.MaMH) != null)
                {
                    nextId++;
                    monHoc.MaMH = "MH" + (nextId + 1);
                }
                db.MonHocs.Add(monHoc);
                MonHocHocKy monHocHocKy = new MonHocHocKy();
                monHocHocKy.MaMH = monHoc.MaMH;
                monHocHocKy.MaHK = Request.Form["MaHK"].ToString();
                monHocHocKy.MaCTDT = Request.Form["MaCTDT"].ToString();
                monHocHocKy.MoTa = "Môn học " + monHoc.MaMH.ToString().Trim() + " " + Request.Form["MaHK"].ToString().Trim();
                db.MonHocHocKies.Add(monHocHocKy);
                db.SaveChanges();
                return Json(new { Success = true });
            } else
            {
                ViewBag.MaCTDT = new SelectList(db.CTDTs, "MaCTDT", "TenCTDT");
                ViewBag.MaHK = new SelectList(db.HocKies, "MaHK", "TenHK");
                StringBuilder message = new StringBuilder();

                foreach (var item in ModelState)
                {
                    var errors = item.Value.Errors;

                    foreach (var error in errors)
                    {
                        message.Append(error.ErrorMessage);
                        message.AppendLine();
                    }
                }
                return Json(new { Success = false, Message = message.ToString() });
            }


        }

        // GET: MonHocs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHocs.Find(id);
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
            MonHoc monHoc = db.MonHocs.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            var monHocHocKy = db.MonHocHocKies.Where(m => m.MaMH == id).FirstOrDefault();
            if (monHocHocKy == null)
            {
                ViewBag.MaCTDT = new SelectList(db.CTDTs, "MaCTDT", "TenCTDT");
                ViewBag.MaHK = new SelectList(db.HocKies, "MaHK", "TenHK");
            } else
            {
                ViewBag.MaCTDT = new SelectList(db.CTDTs, "MaCTDT", "TenCTDT", monHocHocKy.MaCTDT);
                ViewBag.MaHK = new SelectList(db.HocKies, "MaHK", "TenHK", monHocHocKy.MaHK);
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
                var monHocHocKy = db.MonHocHocKies.Where(m => m.MaMH == monHoc.MaMH).FirstOrDefault();
                if (monHocHocKy == null)
                {
                    MonHocHocKy monHocHocKyNew = new MonHocHocKy();
                    monHocHocKyNew.MaMH = monHoc.MaMH;
                    monHocHocKyNew.MaHK = Request.Form["MaHK"].ToString();
                    monHocHocKyNew.MaCTDT = Request.Form["MaCTDT"].ToString();
                    monHocHocKyNew.MoTa = "Môn học " + monHoc.MaMH.ToString().Trim() + " " + Request.Form["MaHK"].ToString().Trim();
                    db.MonHocHocKies.Add(monHocHocKyNew);
                }
                
                db.SaveChanges();
                return Json(new { Success = true });
            } else
            {
                var monHocHocKy = db.MonHocHocKies.Where(m => m.MaMH == monHoc.MaMH).FirstOrDefault();
                if (monHocHocKy == null)
                {
                    ViewBag.MaCTDT = new SelectList(db.CTDTs, "MaCTDT", "TenCTDT");
                    ViewBag.MaHK = new SelectList(db.HocKies, "MaHK", "TenHK");
                }
                else
                {
                    ViewBag.MaCTDT = new SelectList(db.CTDTs, "MaCTDT", "TenCTDT", monHocHocKy.MaCTDT);
                    ViewBag.MaHK = new SelectList(db.HocKies, "MaHK", "TenHK", monHocHocKy.MaHK);
                }
                StringBuilder message = new StringBuilder();
                if (ModelState["SoGio"].Errors.Count > 1)

                    foreach (var item in ModelState)
                    {
                        var errors = item.Value.Errors;

                        foreach (var error in errors)
                        {
                            message.Append(error.ErrorMessage);
                            message.AppendLine();
                        }
                    }
                return Json(new { Success = false, Message = message.ToString() });
            }

        }

        // GET: MonHocs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHocs.Find(id);
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
            MonHoc monHoc = db.MonHocs.Find(id);
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
            MonHoc monHoc = db.MonHocs.Find(id);
            db.MonHocs.Remove(monHoc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("_PartialDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialDeleteConfirmed(string id)
        {
            MonHoc monHoc = db.MonHocs.Find(id);
            var monHocHocKy = db.MonHocHocKies.Where(m => m.MaMH == monHoc.MaMH).FirstOrDefault();
            if (monHocHocKy != null)
            {
               db.MonHocHocKies.Remove(monHocHocKy);
            }
            db.MonHocs.Remove(monHoc);
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