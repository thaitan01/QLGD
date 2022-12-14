using QuanLyGiangDay.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyGiangDay.Controllers
{
    public class LichGiangDayController : Controller
    {
        private readonly quanlygiaovienEntities _context = new quanlygiaovienEntities();
        // GET: LichGiangDay
        public ActionResult Index()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            SelectList giaoviens = Session["loaiGV"].ToString().Trim().Equals("VT01")
                ? new SelectList(_context.GiaoViens.ToList(), "MaGV", "TenGV")
                : new SelectList(_context.GiaoViens.ToList(), "MaGV", "TenGV", Session["maGT"]);
            ViewData["GiaoVienDropdown"] = giaoviens;
            return View();
        }
    }
}