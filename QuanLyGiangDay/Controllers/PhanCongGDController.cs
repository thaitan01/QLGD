using Antlr.Runtime.Tree;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using QuanLyGiangDay.Models.EF;
using QuanLyGiangDay.Models.EFC;
using Rotativa;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyGiangDay.Controllers
{
    public class PhanCongGDController : Controller
    {
        private readonly quanlygiaovienEntities _context = new quanlygiaovienEntities();

        // GET: PhanCongGD
        public ActionResult Index()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.taikhoan = Session["taikhoan"];
            return View();
        }

        [HttpPost]
        public ActionResult GetPhanCongGDTableData(string lopHoc, string monHoc, string gioHoc, string tuNgay, string denNgay, string ngoaiGio, string tatCa)
        {
            JsonResult result = new JsonResult();
            try
            {
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var data = _context.LopHocMonHocs
                            .Join(
                                _context.GiaoViens,
                                lhmh => lhmh.MaGV,
                                gv => gv.MaGV,
                                (lhmh, gv) => new { LHMH = lhmh, GV = gv })
                            .Join(
                                _context.MonHocs,
                                lhmh => lhmh.LHMH.MaMH,
                                mh => mh.MaMH,
                                (lhmh, mh) => new { LHMH = lhmh, MH = mh })
                            .Select(c => new
                            {
                                c.LHMH.LHMH.MaLHMH,
                                c.LHMH.LHMH.MaLop,
                                c.LHMH.LHMH.LopHoc.TenLop,
                                c.MH.TenMon,
                                c.LHMH.LHMH.MaGio,
                                c.LHMH.LHMH.MoTa,
                                c.LHMH.LHMH.NgayKT,
                                c.LHMH.LHMH.NgayBD,
                                c.LHMH.LHMH.NgoaiGio,
                                c.LHMH.GV.TenGV
                            }).ToList();

                if (!string.IsNullOrEmpty(lopHoc))
                {
                    data = data.Where(x => x.MaLop.Contains(lopHoc)).ToList();
                }
                if (!string.IsNullOrEmpty(monHoc))
                {
                    data = data.Where(x => x.TenMon.Contains(monHoc)).ToList();
                }
                if (!string.IsNullOrEmpty(gioHoc))
                {
                    data = data.Where(x => x.MaGio.Contains(gioHoc)).ToList();
                }
                if (!string.IsNullOrEmpty(tuNgay))
                {
                    DateTime cTuNgay = Convert.ToDateTime(tuNgay);
                    data = data.Where(x => x.NgayBD >= (cTuNgay)).ToList();
                }
                if (!string.IsNullOrEmpty(denNgay))
                {
                    DateTime cDenNgay = Convert.ToDateTime(denNgay);
                    data = data.Where(x => x.NgayKT <= (cDenNgay)).ToList();
                }
                if (!string.IsNullOrEmpty(ngoaiGio) && !string.IsNullOrEmpty(tatCa))
                {
                    bool cNgoaiGio = ngoaiGio == "T" ? true : false;
                    if ((ngoaiGio == "T" || ngoaiGio == "F") && tatCa == "F")
                        data = data.Where(x => x.NgoaiGio.Equals(cNgoaiGio)).ToList();
                }

                int totalRecords = data.Count;
                //Sorting
                try
                {
                    switch (order)
                    {
                        case "0":
                            data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MaLHMH).ToList()
                                                                                                     : data.OrderBy(p => p.MaLHMH).ToList();
                            break;
                        case "1":
                            data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MaLop).ToList()
                                                                                                     : data.OrderBy(p => p.MaLop).ToList();
                            break;
                        case "2":
                            data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TenMon).ToList()
                                                                                                     : data.OrderBy(p => p.TenMon).ToList();
                            break;
                        case "3":
                            data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MaGio).ToList()
                                                                                                     : data.OrderBy(p => p.MaGio).ToList();
                            break;
                        case "4":
                            data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MoTa).ToList()
                                                                                                       : data.OrderBy(p => p.MoTa).ToList();
                            break;
                        case "5":
                            data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NgayKT).ToList()
                                                                                                       : data.OrderBy(p => p.NgayKT).ToList();
                            break;
                        case "6":
                            data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NgayBD).ToList()
                                                                                                       : data.OrderBy(p => p.NgayBD).ToList();
                            break;
                        case "7":
                            data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NgoaiGio).ToList()
                                                                                                       : data.OrderBy(p => p.NgoaiGio).ToList();
                            break;
                        default:
                            data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TenGV).ToList()
                                                                                                     : data.OrderBy(p => p.TenGV).ToList();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }

                int recFilter = data.Count;
                data = data.Skip(startRec).Take(pageSize).ToList();

                var modifiedData = data.Select(x => new PhanCongGD()
                {
                    MaLHMH = x.MaLHMH,
                    MaLop = x.MaLop,
                    MaMH = x.TenMon,
                    MaGio = x.MaGio,
                    MoTa = x.MoTa,
                    NgayKT = x.NgayKT.Value.ToString("d/M/yyyy"),
                    NgayBD = x.NgayBD.Value.ToString("d/M/yyyy"),
                    NgoaiGio = x.NgoaiGio == true ? "T" : "F",
                    MaGV = x.TenGV,
                });

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = modifiedData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return result;
        }

        public ActionResult GetPhanCongGDPartial(string id)
        {
            var lhmh = id != null ? _context.LopHocMonHocs.Find(id.Trim()) : new LopHocMonHoc();
            SelectList lophocs;
            if (id != "")
                lophocs = new SelectList(_context.LopHocs.ToList(), "MaLop", "TenLop", lhmh.MaLop);
            else
                lophocs = new SelectList(_context.LopHocs.ToList(), "MaLop", "TenLop");
            ViewData["LopDropdown"] = lophocs;

            SelectList monhocs;
            if (id != "")
                monhocs = new SelectList(_context.MonHocs.ToList(), "MaMH", "TenMon", lhmh.MaLop);
            else
                monhocs = new SelectList(_context.MonHocs.ToList(), "MaMH", "TenMon");
            ViewData["MonHocDropdown"] = monhocs;

            SelectList giaoviens;
            if (id != "")
                giaoviens = new SelectList(_context.GiaoViens.ToList(), "MaGV", "TenGV", lhmh.MaGV);
            else
                giaoviens = new SelectList(_context.GiaoViens.ToList(), "MaGV", "TenGV");
            ViewData["GiaoVienDropdown"] = giaoviens;

            SelectList giohocs;
            if (id != "")
                giohocs = new SelectList(_context.GioHocs.ToList(), "MaGio", "TenGio", lhmh.MaGio);
            else
                giohocs = new SelectList(_context.GioHocs.ToList(), "MaGio", "TenGio");
            ViewData["GioHocDropdown"] = giohocs;

            return PartialView("PhanCongGDPartialView", lhmh);
        }

        public ActionResult GetPhanCongGD()
        {
            SelectList lophocs = new SelectList(_context.LopHocs.ToList(), "MaLop", "TenLop");
            ViewData["LopDropdown"] = lophocs;

            SelectList hockies = new SelectList(_context.HocKies.ToList(), "MaHK", "TenHK");
            ViewData["HocKyDropdown"] = hockies;

            //SelectList monhocs = new SelectList(_context.MonHocs.ToList(), "MaMH", "TenMon");
            //ViewData["MonHocDropdown"] = monhocs;

            SelectList giohocs = new SelectList(_context.GioHocs.ToList(), "MaGio", "TenGio");
            ViewData["GioHocDropdown"] = giohocs;

            return View("PhanCongGDView");
        }

        public ActionResult GetBaoCaoGD()
        {
            return View("BaoCaoGDView");
        }

        [HttpPost]
        public ActionResult GetBaoCaoGDTableData(string tuNgay, string denNgay)
        {
            JsonResult result = new JsonResult();
            try
            {
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                //string order = Request.Form.GetValues("order[0][column]")[0];
                //string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                DateTime cTuNgay = Convert.ToDateTime(tuNgay);
                DateTime cDenNgay = Convert.ToDateTime(denNgay);
                var data = _context.LopHocMonHocs
                            .Join(
                                _context.GiaoViens,
                                lhmh => lhmh.MaGV,
                                gv => gv.MaGV,
                                (lhmh, gv) => new { LHMH = lhmh, GV = gv })
                            .Join(
                                _context.MonHocs,
                                lhmh => lhmh.LHMH.MaMH,
                                mh => mh.MaMH,
                                (lhmh, mh) => new { LHMH = lhmh, MH = mh })
                            .Where(c => c.LHMH.LHMH.NgayBD >= cTuNgay)
                            .Where(c => c.LHMH.LHMH.NgayBD <= cDenNgay)
                            .GroupBy(c => c.LHMH.LHMH.MaGV)
                            .Select(a => new { sum = a.Sum(b => b.LHMH.LHMH.MonHoc.SoGio), maGV = a.Key })
                            .ToList();
                var newData = new List<PhanCongGD>();
                for (int i = 0; i < data.Count; i++)
                {
                    var model = new PhanCongGD();
                    model.MaGV = _context.GiaoViens.Find(data[i].maGV).TenGV;
                    var trongGio = _context.LopHocMonHocs
                            .Join(
                                _context.GiaoViens,
                                lhmh => lhmh.MaGV,
                                gv => gv.MaGV,
                                (lhmh, gv) => new { LHMH = lhmh, GV = gv })
                            .Join(
                                _context.MonHocs,
                                lhmh => lhmh.LHMH.MaMH,
                                mh => mh.MaMH,
                                (lhmh, mh) => new { LHMH = lhmh, MH = mh }).AsEnumerable()
                            .Where(c => c.LHMH.LHMH.MaGV == data[i].maGV)
                            .Where(c => c.LHMH.LHMH.NgoaiGio == false)
                            .GroupBy(c => c.LHMH.LHMH.MaGV)
                            .Select(a => new { sum = a.Sum(b => b.LHMH.LHMH.MonHoc.SoGio), maGV = a.Key })
                            .ToList();
                    model.TrongGio = trongGio.Count > 0 ? trongGio[0].sum.ToString() : "0";
                    var ngoaiGio = _context.LopHocMonHocs
                            .Join(
                                _context.GiaoViens,
                                lhmh => lhmh.MaGV,
                                gv => gv.MaGV,
                                (lhmh, gv) => new { LHMH = lhmh, GV = gv })
                            .Join(
                                _context.MonHocs,
                                lhmh => lhmh.LHMH.MaMH,
                                mh => mh.MaMH,
                                (lhmh, mh) => new { LHMH = lhmh, MH = mh }).AsEnumerable()
                            .Where(c => c.LHMH.LHMH.MaGV == data[i].maGV)
                            .Where(c => c.LHMH.LHMH.NgoaiGio)
                            .GroupBy(c => c.LHMH.LHMH.MaGV)
                            .Select(a => new { sum = a.Sum(b => b.LHMH.LHMH.MonHoc.SoGio), maGV = a.Key })
                            .ToList();
                    model.NgoaiGio = ngoaiGio.Count > 0 ? ngoaiGio[0].sum.ToString() : "0";
                    model.Tong = data[i].sum.ToString();
                    newData.Add(model);
                }

                int recFilter = newData.Count;
                newData = newData.Skip(startRec).Take(pageSize).ToList();

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = newData.Count,
                    recordsFiltered = recFilter,
                    data = newData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return result;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetGiaoVienByLopHocMonHoc(string lopHoc, string monHoc, string gioHoc, string ngayBD, string ngayKT)
        {
            var giaoviens = GetAllGiaoVienByLopMon(lopHoc, monHoc, gioHoc, ngayBD, ngayKT);
            return Json(giaoviens, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetMonHocByLopHocHocKy(string lopHoc, string hocKy)
        {
            var hockies = GetAllMonHocByLopHocHocKy(lopHoc, hocKy);
            return Json(hockies, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllGiaoVienByLopMon(string lopHoc, string monHoc, string gioHoc, string ngayBD, string ngayKT)
        {
            //generate empty list
            var selectList = new List<SelectListItem>();

            DateTime bd = Convert.ToDateTime(ngayBD);
            DateTime kt = Convert.ToDateTime(ngayKT);

            var listOfLHMH = _context.LopHocMonHocs.Where(x => x.MaLop.Trim().Equals(lopHoc.Trim()))
                .Where(x => x.MaMH.Trim().Equals(monHoc.Trim()))
                .Where(x => x.MaGio.Trim().Equals(gioHoc.Trim()))
                .Where(x => x.NgayBD >= bd)
                .Where(x => x.NgayBD <= kt)
                .Select(x => x.MaGV).ToList();

            var giaoViens = _context.GiaoViens.Where(r => !listOfLHMH.Contains(r.MaGV))
                .Select(c => new {
                    c.MaGV,
                    c.TenGV
                }).ToList();

            foreach (var gv in giaoViens)
            {
                //add elements in dropdown
                selectList.Add(new SelectListItem
                {
                    Value = gv.MaGV.ToString(),
                    Text = gv.TenGV.ToString()
                });
            }
            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllMonHocByLopHocHocKy(string lopHoc, string hocKy)
        {
            //generate empty list
            var selectList = new List<SelectListItem>();
            
            var ctdt = _context.LopHocs.Where(x => x.MaLop.Trim().Equals(lopHoc.Trim()))
                .Select(x => x.MaCTDT).FirstOrDefault();

            var listOfMonHoc = _context.MonHocHocKies.Where(x => x.MaCTDT.Trim().Equals(ctdt.Trim()))
                .Where(y => y.MaHK.Trim().Equals(hocKy.Trim()))
               .Select(c => new {
                   c.MaMH,
                   c.MonHoc.TenMon
               }).ToList();

            foreach (var mh in listOfMonHoc)
            {
                //add elements in dropdown
                selectList.Add(new SelectListItem
                {
                    Value = mh.MaMH.ToString(),
                    Text = mh.TenMon.ToString()
                });
            }
            return selectList;
        }

        public ActionResult SavePhanCongGD(LopHocMonHoc _obj)
        {
            if (ModelState.IsValid)
            {
                if (_obj.MaLHMH != null)
                {
                    _context.Entry(_obj).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    var isExist = _context.LopHocMonHocs.Where(x => x.MaLop == _obj.MaLop)
                       .Where(x => x.MaMH == _obj.MaMH)
                       .Where(x => x.MaGio == _obj.MaGio).ToList();
                    if (isExist.Count > 0)
                    {
                        return Json(new { Message = "E|Lớp học đã được phân công trước đó!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var countOfRows = _context.LopHocMonHocs.Count();
                        var lastRow = _context.LopHocMonHocs.OrderBy(c => c.MaLHMH).Skip(countOfRows - 1).FirstOrDefault();
                        int nextId = Convert.ToInt32(lastRow.MaLHMH.Substring(4));
                        _obj.MaLHMH = "LHMH" + (nextId + 1);
                        _context.LopHocMonHocs.Add(_obj);
                    }
                }
                _context.SaveChanges();
                return Json(new { Message = "S|Lưu dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = "E|Đã xãy ra lỗi trong quá trình lưu dữ liệu!" }, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult CheckGiaoVienHasChungChi(string giaovien, string monHoc)
        {
            var listOfChungChi = _context.GiaoVienMonHocs.Where(x => x.MaGV.Trim().Equals(giaovien.Trim()))
                .Where(x => x.MaMH.Trim().Equals(monHoc.Trim()))
                .Select(x => x.MaMH).ToList();
            if(listOfChungChi.Count > 0)
                return Json("T", JsonRequestBehavior.AllowGet);
            else
                return Json("F", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePhanCongGD(string id)
        {
            try
            {
                var lhmh = _context.LopHocMonHocs.Find(id);
                _context.LopHocMonHocs.Remove(lhmh);
                _context.SaveChanges();
                return Json(new { Message = "Xóa dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Xảy ra lỗi trong quá trình xóa dữ liệu!" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult PrintPDF(string lopHoc, string monHoc, string gioHoc, string tuNgay, string denNgay, string ngoaiGio, string tatCa)
        {
            PhanCongModel mainModel = new PhanCongModel();
            DateTime cTuNgay, cDenNgay;
            if (!string.IsNullOrEmpty(tuNgay))
                cTuNgay = Convert.ToDateTime(tuNgay);
            else
                cTuNgay = DateTime.MinValue;

            if (!string.IsNullOrEmpty(denNgay))
                cDenNgay = Convert.ToDateTime(denNgay);
            else
                cDenNgay = DateTime.MaxValue;

            var data = _context.LopHocMonHocs
                            .Join(
                                _context.GiaoViens,
                                lhmh => lhmh.MaGV,
                                gv => gv.MaGV,
                                (lhmh, gv) => new { LHMH = lhmh, GV = gv })
                            .Join(
                                _context.MonHocs,
                                lhmh => lhmh.LHMH.MaMH,
                                mh => mh.MaMH,
                                (lhmh, mh) => new { LHMH = lhmh, MH = mh })
                            .Select(c => new
                            {
                                c.LHMH.LHMH.MonHoc.SoGio,
                                c.LHMH.LHMH.MaLop,
                                c.LHMH.LHMH.LopHoc.TenLop,
                                c.MH.TenMon,
                                c.LHMH.LHMH.MaGio,
                                c.LHMH.LHMH.MoTa,
                                c.LHMH.LHMH.NgayKT,
                                c.LHMH.LHMH.NgayBD,
                                c.LHMH.LHMH.NgoaiGio,
                                c.LHMH.GV.TenGV
                            }).ToList();

            if (!string.IsNullOrEmpty(lopHoc))
            {
                data = data.Where(x => x.TenLop.Contains(lopHoc)).ToList();
            }
            if (!string.IsNullOrEmpty(monHoc))
            {
                data = data.Where(x => x.TenMon.Contains(monHoc)).ToList();
            }
            if (!string.IsNullOrEmpty(gioHoc))
            {
                data = data.Where(x => x.MaGio.Contains(gioHoc)).ToList();
            }
            if (!string.IsNullOrEmpty(tuNgay))
            {
                cTuNgay = Convert.ToDateTime(tuNgay);
                data = data.Where(x => x.NgayBD >= (cTuNgay)).ToList();
            }
            if (!string.IsNullOrEmpty(denNgay))
            {
                cDenNgay = Convert.ToDateTime(denNgay);
                data = data.Where(x => x.NgayKT <= (cDenNgay)).ToList();
            }
            if (!string.IsNullOrEmpty(ngoaiGio) && !string.IsNullOrEmpty(tatCa))
            {
                bool cNgoaiGio = ngoaiGio == "T" ? true : false;
                if ((ngoaiGio == "T" || ngoaiGio == "F") && tatCa == "F")
                    data = data.Where(x => x.NgoaiGio.Equals(cNgoaiGio)).ToList();
            }
            var modifiedData = data.Select(x => new PhanCongGD
            {
                MaLHMH = x.SoGio == null ? "0" : x.SoGio.ToString(),
                MaLop = x.MaLop,
                MaMH = x.TenMon,
                MaGio = x.MaGio,
                MoTa = x.MoTa,
                NgayKT = x.NgayBD.Value.ToString("M/yyyy"),
                NgayBD = x.NgayBD.Value.ToString("d/M/yyyy"),
                NgoaiGio = x.NgoaiGio == true ? "T" : "F",
                MaGV = x.TenGV,
            }).ToList();

            //Tong hop gio giang
            var data2 = _context.LopHocMonHocs
                        .Join(
                            _context.GiaoViens,
                            lhmh => lhmh.MaGV,
                            gv => gv.MaGV,
                            (lhmh, gv) => new { LHMH = lhmh, GV = gv })
                        .Join(
                            _context.MonHocs,
                            lhmh => lhmh.LHMH.MaMH,
                            mh => mh.MaMH,
                            (lhmh, mh) => new { LHMH = lhmh, MH = mh })
                        .Where(c => c.LHMH.LHMH.NgayBD >= cTuNgay)
                        .Where(c => c.LHMH.LHMH.NgayBD <= cDenNgay)
                        .GroupBy(c => c.LHMH.LHMH.MaGV)
                        .Select(a => new { sum = a.Sum(b => b.LHMH.LHMH.MonHoc.SoGio), maGV = a.Key })
                        .ToList();
            var newData = new List<PhanCongGD>();
            for (int i = 0; i < data2.Count; i++)
            {
                var model = new PhanCongGD();
                model.MaGV = _context.GiaoViens.Find(data2[i].maGV).TenGV;
                var trongGio = _context.LopHocMonHocs
                        .Join(
                            _context.GiaoViens,
                            lhmh => lhmh.MaGV,
                            gv => gv.MaGV,
                            (lhmh, gv) => new { LHMH = lhmh, GV = gv })
                        .Join(
                            _context.MonHocs,
                            lhmh => lhmh.LHMH.MaMH,
                            mh => mh.MaMH,
                            (lhmh, mh) => new { LHMH = lhmh, MH = mh }).AsEnumerable()
                        .Where(c => c.LHMH.LHMH.MaGV == data2[i].maGV)
                        .Where(c => c.LHMH.LHMH.NgoaiGio == false)
                        .GroupBy(c => c.LHMH.LHMH.MaGV)
                        .Select(a => new { sum = a.Sum(b => b.LHMH.LHMH.MonHoc.SoGio), maGV = a.Key })
                        .ToList();
                model.TrongGio = trongGio.Count > 0 ? trongGio[0].sum.ToString() : "0";
                var ngoaiGio2 = _context.LopHocMonHocs
                        .Join(
                            _context.GiaoViens,
                            lhmh => lhmh.MaGV,
                            gv => gv.MaGV,
                            (lhmh, gv) => new { LHMH = lhmh, GV = gv })
                        .Join(
                            _context.MonHocs,
                            lhmh => lhmh.LHMH.MaMH,
                            mh => mh.MaMH,
                            (lhmh, mh) => new { LHMH = lhmh, MH = mh }).AsEnumerable()
                        .Where(c => c.LHMH.LHMH.MaGV == data2[i].maGV)
                        .Where(c => c.LHMH.LHMH.NgoaiGio)
                        .GroupBy(c => c.LHMH.LHMH.MaGV)
                        .Select(a => new { sum = a.Sum(b => b.LHMH.LHMH.MonHoc.SoGio), maGV = a.Key })
                        .ToList();
                model.NgoaiGio = ngoaiGio2.Count > 0 ? ngoaiGio2[0].sum.ToString() : "0";
                model.Tong = data2[i].sum.ToString();
                newData.Add(model);
            }

            mainModel.PhanCong = modifiedData;
            mainModel.TongHop = newData;
            if (!string.IsNullOrEmpty(tuNgay))
                mainModel.NgayPC = cTuNgay.ToString("M/yyyy");
            else
                mainModel.NgayPC = DateTime.Today.ToString("M/yyyy");

            mainModel.NgayXuatBC = "Ngày " + DateTime.Today.Day.ToString() + " tháng " + DateTime.Today.Month.ToString() +
                                    " năm " + DateTime.Today.Year.ToString();

            return new PartialViewAsPdf("PrintPdf", null)
            {
                FileName = "BangPhanCongGiangDay" + DateTime.Today.ToString("d-M-yyyy") + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                Model = mainModel
            };
        }

        public FileResult ExportToExcel(string lopHoc, string monHoc, string gioHoc, string tuNgay, string denNgay, string ngoaiGio, string tatCa)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("Tháng"),
                                                     new DataColumn("Ngày bắt đầu"),
                                                     new DataColumn("Mã môn"),
                                                     new DataColumn("Tên môn"),
                                                     new DataColumn("Lớp"),
                                                     new DataColumn("Giờ học"),
                                                     new DataColumn("Học kỳ"),
                                                     new DataColumn("Phân công giảng dạy")});

            DateTime cTuNgay, cDenNgay;
            if (!string.IsNullOrEmpty(tuNgay))
                cTuNgay = Convert.ToDateTime(tuNgay);
            else
                cTuNgay = DateTime.MinValue;

            if (!string.IsNullOrEmpty(denNgay))
                cDenNgay = Convert.ToDateTime(denNgay);
            else
                cDenNgay = DateTime.MaxValue;

            var data = _context.LopHocMonHocs
                            .Join(
                                _context.GiaoViens,
                                lhmh => lhmh.MaGV,
                                gv => gv.MaGV,
                                (lhmh, gv) => new { LHMH = lhmh, GV = gv })
                            .Join(
                                _context.MonHocs,
                                lhmh => lhmh.LHMH.MaMH,
                                mh => mh.MaMH,
                                (lhmh, mh) => new { LHMH = lhmh, MH = mh })
                            .Select(c => new
                            {
                                c.LHMH.LHMH.MonHoc.SoGio,
                                c.LHMH.LHMH.MaLop,
                                c.LHMH.LHMH.MaMH,
                                c.LHMH.LHMH.LopHoc.TenLop,
                                c.MH.TenMon,
                                c.LHMH.LHMH.MaGio,
                                c.LHMH.LHMH.MoTa,
                                c.LHMH.LHMH.NgayKT,
                                c.LHMH.LHMH.NgayBD,
                                c.LHMH.LHMH.NgoaiGio,
                                c.LHMH.GV.TenGV,
                                c.MH.MonHocHocKies.FirstOrDefault().HocKy.TenHK
                            }).ToList();

            if (!string.IsNullOrEmpty(lopHoc))
            {
                data = data.Where(x => x.TenLop.Contains(lopHoc)).ToList();
            }
            if (!string.IsNullOrEmpty(monHoc))
            {
                data = data.Where(x => x.TenMon.Contains(monHoc)).ToList();
            }
            if (!string.IsNullOrEmpty(gioHoc))
            {
                data = data.Where(x => x.MaGio.Contains(gioHoc)).ToList();
            }
            if (!string.IsNullOrEmpty(tuNgay))
            {
                cTuNgay = Convert.ToDateTime(tuNgay);
                data = data.Where(x => x.NgayBD >= (cTuNgay)).ToList();
            }
            if (!string.IsNullOrEmpty(denNgay))
            {
                cDenNgay = Convert.ToDateTime(denNgay);
                data = data.Where(x => x.NgayKT <= (cDenNgay)).ToList();
            }
            if (!string.IsNullOrEmpty(ngoaiGio) && !string.IsNullOrEmpty(tatCa))
            {
                bool cNgoaiGio = ngoaiGio == "T" ? true : false;
                if ((ngoaiGio == "T" || ngoaiGio == "F") && tatCa == "F")
                    data = data.Where(x => x.NgoaiGio.Equals(cNgoaiGio)).ToList();
            }
            var modifiedData = data.Select(x => new PhanCongGD
            {
                MaLHMH = x.SoGio == null ? "0" : x.SoGio.ToString(),
                MaLop = x.MaLop,
                MaMon = x.MaMH,
                MaMH = x.TenMon,
                MaGio = x.MaGio,
                MoTa = x.TenHK,
                NgayKT = x.NgayBD.Value.ToString("M/yyyy"),
                NgayBD = x.NgayBD.Value.ToString("d/M/yyyy"),
                NgoaiGio = x.NgoaiGio == true ? "T" : "F",
                MaGV = x.TenGV,
            }).ToList();

            foreach (var item in modifiedData)
            {
                dt.Rows.Add(item.NgayKT, item.NgayBD, item.MaMon, item.MaMH,
                    item.MaLop, item.MaGio, item.MoTa, item.MaGV);
            }

            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelFile.xlsx");
                }
            }
        }
    }
}