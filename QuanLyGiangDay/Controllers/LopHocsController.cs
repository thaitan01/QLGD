using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using QuanLyGiangDay.Models.EF;

namespace QuanLyGiangDay.Controllers
{
    public class LopHocsController : Controller
    {
        private quanlygiaovienEntities db = new quanlygiaovienEntities();

        // GET: LopHocs
        public ActionResult Index()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var lopHocs = db.LopHoc.Include(l => l.CTDT);
            return View(lopHocs.ToList());
        }

        // GET: LopHocs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHoc lopHoc = db.LopHoc.Find(id);
            if (lopHoc == null)
            {
                return HttpNotFound();
            }
            return View(lopHoc);
        }

        
        public ActionResult _PartialDetail(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var result = db.LopHoc.Where(l => l.MaLop == id)
                                   .Join(
                                        db.LopHocMonHoc,
                                        lh => lh.MaLop,
                                        lhmh => lhmh.MaLop,
                                        (lh, lhmh) => new { LH = lh, LHMH = lhmh })
                                   .Join(
                                        db.MonHocHocKy,
                                        lh => lh.LHMH.MaMH,
                                        mhhk => mhhk.MaMH,
                                        (lh, mhhk) => new { LH = lh, MHHk = mhhk })
                                   .Select(c => new
                                   {
                                       c.MHHk.MonHoc.TenMon,
                                       c.MHHk.HocKy.TenHK,
                                       c.LH.LHMH.NgayBD
                                   }).ToList();
            List<MonHocHocKyModel> listMH = new List<MonHocHocKyModel>();
            foreach (var item in result)
            {
                MonHocHocKyModel m = new MonHocHocKyModel(item.TenHK, item.TenMon, item.NgayBD.Value.ToString("dd/MM/yyyy"));
                listMH.Add(m);
            }

            LopHoc lopHoc = db.LopHoc.Find(id);
            ViewBag.LopHoc = lopHoc.TenLop;
            ViewBag.MHHK = listMH;
            if (listMH.Count == 0) ViewBag.EmptyData = true;
            return PartialView("_PartialDetail");
        }




        // GET: LopHocs/Create
        public ActionResult Create()
        {
            ViewBag.MaCTDT = new SelectList(db.CTDT, "MaCTDT", "TenCTDT");
            return View();
        }

        public ActionResult _PartialCreate()
        {
            ViewBag.MaCTDT = new SelectList(db.CTDT, "MaCTDT", "TenCTDT");
            return PartialView("_PartialCreate");
        }

        // POST: LopHocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLop,MaCTDT,TenLop,MoTa")] LopHoc lopHoc)
        {
           
            if (ModelState.IsValid)
            {
                db.LopHoc.Add(lopHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaCTDT = new SelectList(db.CTDT, "MaCTDT", "TenCTDT", lopHoc.MaCTDT);
            return View(lopHoc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialCreate([Bind(Include = "MaLop,MaCTDT,TenLop,MoTa")] LopHoc lopHoc)
        {
            
            if (ModelState.IsValid)
            {
                db.LopHoc.Add(lopHoc);
                db.SaveChanges();
                return Json(new { Success = true });
            } else {
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


        // GET: LopHocs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHoc lopHoc = db.LopHoc.Find(id);
            if (lopHoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCTDT = new SelectList(db.CTDT, "MaCTDT", "TenCTDT", lopHoc.MaCTDT);
            return View(lopHoc);
        }

        public ActionResult _PartialEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHoc lopHoc = db.LopHoc.Find(id);
            if (lopHoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCTDT = new SelectList(db.CTDT, "MaCTDT", "TenCTDT", lopHoc.MaCTDT);
            return PartialView("_PartialEdit", lopHoc);
        }

        // POST: LopHocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLop,MaCTDT,TenLop,MoTa")] LopHoc lopHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lopHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaCTDT = new SelectList(db.CTDT, "MaCTDT", "TenCTDT", lopHoc.MaCTDT);
            return View(lopHoc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialEdit([Bind(Include = "MaLop,MaCTDT,TenLop,MoTa")] LopHoc lopHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lopHoc).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Success = true });
            } else
            {
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

        // GET: LopHocs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHoc lopHoc = db.LopHoc.Find(id);
            if (lopHoc == null)
            {
                return HttpNotFound();
            }
            return View(lopHoc);
        }

        // POST: LopHocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LopHoc lopHoc = db.LopHoc.Find(id);
            db.LopHoc.Remove(lopHoc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("_PartialDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialDeleteConfirmed(string id)
        {
            LopHoc lopHoc = db.LopHoc.Find(id);
            db.LopHoc.Remove(lopHoc);
            db.SaveChanges();
            return Json(new { Success = true });
        }

        public ActionResult _PartialDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHoc lopHoc = db.LopHoc.Find(id);
            if (lopHoc == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialDelete", lopHoc);
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

    public class MonHocHocKyModel
    {
        
        public string MaMH { get; set; }
        public string MaHK { get; set; }
        public string NgayBD { get; set; }

        public MonHocHocKyModel(string TenHK, string TenMon, string NgayBD)
        {
            
            this.MaMH = TenMon;
            this.MaHK = TenHK;
            this.NgayBD = NgayBD;
        }
        
    }

    public class GiaoVienGioDayModel
    {

        public string MaGV { get; set; }
        public decimal NgoaiGio { get; set; }
        public decimal TrongGio { get; set; }

        public GiaoVienGioDayModel(string MaGV, decimal NgoaiGio, decimal TrongGio)
        {

            this.MaGV = MaGV;
            this.NgoaiGio = NgoaiGio;
            this.TrongGio = TrongGio;
        }

    }

}
