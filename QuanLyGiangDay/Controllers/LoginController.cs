using QuanLyGiangDay.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLyGiangDay.Controllers
{
    public class LoginController : Controller
    {
        private quanlygiaovienEntities db = new quanlygiaovienEntities();

        // GET: Login
        public ActionResult Index()
        {
            if (Session["taikhoan"] != null)
            {
               return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "TenDN, MatKhau")] LoginModel login)
        {

            if (ModelState.IsValid)
            {
                var result = db.TaiKhoan.Where(tk => tk.TenDN == login.TenDN).FirstOrDefault();

                login.MatKhau = Crypto.Hash(login.MatKhau, "MD5");
                if (result == null)
                {
                    
                    ViewBag.Message = "Tên đăng nhập không tồn tại!";
                    return View("Index");

                }
                else if (result.MatKhau == login.MatKhau)
                {
                    Session["taikhoan"] = db.GiaoVien.Find(result.MaGV).TenGV;
                    return RedirectToAction("Index", "LichGiangDay");
                }
                else
                {
                    ViewBag.Message = "Mật khẩu không đúng!";
                    return View("Index");
                }
            } else
            {

                return View("Index");
            }
            

        }

        public ActionResult Logout()
        {
            Session["taikhoan"] = null;
            return RedirectToAction("Index","Login");
        }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được rỗng")]
        public string TenDN { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được rỗng")]
        public string MatKhau { get; set; }
    }

}