using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyGiangDay.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.taikhoan = Session["taikhoan"];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
			//Test 01
            return View();
        }
    }
}