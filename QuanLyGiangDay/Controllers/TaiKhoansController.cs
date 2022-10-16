using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyGiangDay.Models.EF;
using System.Web.Helpers;
using System.Text;
using System.Data.Entity.Validation;

namespace QuanLyGiangDay.Controllers
{
    public class TaiKhoansController : Controller
    {
        private quanlygiaovienEntities db = new quanlygiaovienEntities();

        // GET: TaiKhoans
        public ActionResult Index()
        {
            var taiKhoan = db.TaiKhoan.Include(t => t.VaiTro);
            return View(taiKhoan.ToList());
        }

        // GET: TaiKhoans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        public ActionResult _PartialDetail(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialDetail",taiKhoan);
        }

        // GET: TaiKhoans/Create
        public ActionResult Create()
        {
            ViewBag.MaVT = new SelectList(db.VaiTro, "MaVT", "TenVT");
            return View();
        }

        public ActionResult _PartialCreate()
        {
            Dictionary<string, string> listGV = new Dictionary<string, string>();
            foreach (GiaoVien gv in db.GiaoVien)
            {
                string value = gv.MaGV + " - " + gv.TenGV;
                listGV.Add(gv.MaGV, value);
            }

            ViewBag.MaGV = new SelectList(listGV, "key", "value");
            ViewBag.MaVT = new SelectList(db.VaiTro, "MaVT", "TenVT");
            return PartialView("_PartialCreate");
        }

        // POST: TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenDN,MatKhau,MaGV,MaVT,HoTen")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
               
                db.TaiKhoan.Add(taiKhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            } 
            ViewBag.MaGV = new SelectList(db.GiaoVien, "MaGV", "MaGV", taiKhoan.MaGV);
            ViewBag.MaVT = new SelectList(db.VaiTro, "MaVT", "TenVT", taiKhoan.MaVT);
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialCreate([Bind(Include = "TenDN,MatKhau,MaGV,MaVT,HoTen")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
              
                var result = db.TaiKhoan.Where(tk => tk.TenDN == taiKhoan.TenDN).FirstOrDefault();
                var hasTaiKhoan = db.TaiKhoan.Where(tk => tk.MaGV == taiKhoan.MaGV && tk.MaVT == taiKhoan.MaVT).FirstOrDefault();

                    if ((result == null) && (hasTaiKhoan == null))
                    {
                        var countOfRows = db.TaiKhoan.Count();
                    
                        if (countOfRows == 0)
                        {
                            taiKhoan.MaTK = "TK1";
                            taiKhoan.MatKhau = Crypto.Hash(taiKhoan.MatKhau, "MD5");
                            taiKhoan.VaiTro = db.VaiTro.Find(taiKhoan.MaVT);
                            db.TaiKhoan.Add(taiKhoan);
                            db.SaveChanges();
                            return Json(new { Success = true });
                        } else
                        {
                            var lastRow = db.TaiKhoan.OrderBy(c => c.MaTK).Skip(countOfRows - 1).FirstOrDefault();
                            int nextId = Convert.ToInt32(lastRow.MaTK.Substring(2));
                            taiKhoan.MaTK = "TK" + (nextId + 1);
                            taiKhoan.MatKhau = Crypto.Hash(taiKhoan.MatKhau, "MD5");
                            taiKhoan.VaiTro = db.VaiTro.Find(taiKhoan.MaVT);
                            db.TaiKhoan.Add(taiKhoan);
                            db.SaveChanges();
                            return Json(new { Success = true });
                        }
                      
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "Tên đăng nhập đã tồn tại hoặc giáo viên này đã có tài khoản tương tự! Không thể thêm mới" });
                    }
                    
                
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

        // GET: TaiKhoans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.MaVT = new SelectList(db.VaiTro, "MaVT", "TenVT", taiKhoan.MaVT);
            return View(taiKhoan);
        }

        public ActionResult _PartialEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            Dictionary<string, string> listGV = new Dictionary<string, string>();
            foreach (GiaoVien gv in db.GiaoVien)
            {
                string value = gv.MaGV + " - " + gv.TenGV;
                listGV.Add(gv.MaGV, value);
            }

            ViewBag.MaGV = new SelectList(listGV, "key", "value", taiKhoan.MaGV);
            ViewBag.MaVT = new SelectList(db.VaiTro, "MaVT", "TenVT", taiKhoan.MaVT);
            return PartialView("_PartialEdit", taiKhoan);
        }

        // POST: TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTK,TenDN,MatKhau,MaGV,MaVT,HoTen")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaGV = new SelectList(db.GiaoVien, "MaGV", "MaGV", taiKhoan.MaGV);
            ViewBag.MaVT = new SelectList(db.VaiTro, "MaVT", "TenVT", taiKhoan.MaVT);
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialEdit([Bind(Include = "MaTK,TenDN,MatKhau,MaGV,MaVT,HoTen")] TaiKhoan taiKhoan)
        {
            ViewBag.MaGV = new SelectList(db.GiaoVien, "MaGV", "MaGV", taiKhoan.MaGV);
            ViewBag.MaVT = new SelectList(db.VaiTro, "MaVT", "TenVT", taiKhoan.MaVT);
            if (ModelState.IsValid)
            {
                
                var hasTaiKhoan = db.TaiKhoan.Where(tk => tk.MaGV == taiKhoan.MaGV && tk.MaVT == taiKhoan.MaVT).FirstOrDefault();
                if ( hasTaiKhoan == null)
                {
                    db.Entry(taiKhoan).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { Success = true });
                } else
                {
                    return Json(new { Success = false, Message = "Tên đăng nhập đã tồn tại hoặc giáo viên này đã có tài khoản tương tự! Không thể cập nhật" });
                }
                
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

        // GET: TaiKhoans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        public ActionResult _PartialDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialDelete", taiKhoan);
        }

        // POST: TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            db.TaiKhoan.Remove(taiKhoan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("_PartialDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialDeleteConfirmed(string id)
        {
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            db.TaiKhoan.Remove(taiKhoan);
            db.SaveChanges();
            return Json(new { Success = true });
        }


        public ActionResult _PartialReset(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialReset", taiKhoan);
        }

        [HttpPost, ActionName("_PartialReset")]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialResetConfirmed(string id)
        {
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            taiKhoan.MatKhau = Crypto.Hash("1234567", "MD5");
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
