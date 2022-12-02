using QuanLyGiangDay.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QuanLyGiangDay.Controllers
{
    public class CertificateController : Controller
    {
        private quanlygiaovienEntities db = new quanlygiaovienEntities();
        public static string MaGV { get; set; }
        // GET: Certificate
        public ActionResult Certificate(string id)
        {
            var chungChi = db.GiaoVienMonHocs.Where(c => c.MaGV == id).ToList();
            List<GiaoVienMonHoc> listChungChi = new List<GiaoVienMonHoc>();
            foreach (var item in chungChi)
            {
                listChungChi.Add(item);
            }
            // TempData["MaGV"] = db.GiaoViens.Find(id).MaGV;
            MaGV = db.GiaoViens.Find(id).MaGV.ToString();
            ViewBag.TenGiaoVien = db.GiaoViens.Find(id).TenGV;
            ViewBag.ChungChi = listChungChi;
            ViewBag.SoChungChi = chungChi.Count();
            return View();
        }

        public ActionResult _PartialCreate()
        {
            ViewBag.MaMH = new SelectList(db.MonHocs, "MaMH", "TenMon");
            GiaoVienMonHoc giaoVienMonHoc = new GiaoVienMonHoc();
            giaoVienMonHoc.MaGV = MaGV;
            return PartialView("_PartialCreate", giaoVienMonHoc);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialCreate([Bind(Include = "MaGV, MaMH, MoTa")] GiaoVienMonHoc giaoVienMonHoc)
        {
            var result = db.GiaoVienMonHocs.Where(gv => gv.MaGV == giaoVienMonHoc.MaGV).Where(gv => gv.MaMH == giaoVienMonHoc.MaMH).FirstOrDefault();
            if (result == null)
            {
                if (ModelState.IsValid)
                {
                    if (giaoVienMonHoc.MoTa == "" || giaoVienMonHoc.MoTa == null)
                    {
                        giaoVienMonHoc.MoTa = "Đạt";
                    } else
                    {
                        giaoVienMonHoc.MoTa = "Đạt (" + giaoVienMonHoc.MoTa.ToString() + ")";
                    }      
                    db.GiaoVienMonHocs.Add(giaoVienMonHoc);
                    db.SaveChanges();
                    return Json(new { Success = true });
                }
                else
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
            } else
            {
                return Json(new { Success = false, Message = "Giáo viên này đã có chứng chỉ môn học vừa chọn. Không thể thêm mới!" });
            }
            
        }

        public ActionResult _PartialEdit(string MaGV, string MaMH)
        {
            ViewBag.MaMH = new SelectList(db.MonHocs, "MaMH", "TenMon");
            GiaoVienMonHoc giaoVienMonHoc = db.GiaoVienMonHocs.Where(c =>  c.MaMH == MaMH).Where(c => c.MaGV == MaGV).FirstOrDefault();
            return PartialView("_PartialEdit", giaoVienMonHoc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialEdit([Bind(Include = "MaGV,MaMH,MoTa")] GiaoVienMonHoc giaoVienMonHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(giaoVienMonHoc).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Success = true });
            } else
            {
               ViewBag.MaGV = new SelectList(db.GiaoViens, "MaGV", "TenGV", giaoVienMonHoc.MaGV);
               ViewBag.MaMH = new SelectList(db.MonHocs, "MaMH", "TenMon", giaoVienMonHoc.MaMH);
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

        public ActionResult _PartialDelete(string MaGV, string MaMH)
        {
            ViewBag.MaMH = new SelectList(db.MonHocs, "MaMH", "TenMon");
            GiaoVienMonHoc giaoVienMonHoc = db.GiaoVienMonHocs.Where(c => c.MaMH == MaMH).Where(c => c.MaGV == MaGV).FirstOrDefault();
            return PartialView("_PartialDelete", giaoVienMonHoc);
        }

        [HttpPost, ActionName("_PartialDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialDeleteConfirmed(string MaGV, string MaMH)
        {
            GiaoVienMonHoc giaoVienMonHoc = db.GiaoVienMonHocs.Where(c => c.MaMH == MaMH).Where(c => c.MaGV == MaGV).FirstOrDefault();
            db.GiaoVienMonHocs.Remove(giaoVienMonHoc);
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