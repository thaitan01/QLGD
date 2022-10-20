using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyGiangDay.Controllers
{
    public class LichGiangDayController : Controller
    {
        // GET: LichGiangDay
        public ActionResult Index()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.taikhoan = Session["taikhoan"];
            return View();
        }
    }
}