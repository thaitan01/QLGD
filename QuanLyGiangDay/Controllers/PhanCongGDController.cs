using QuanLyGiangDay.Models.EF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace QuanLyGiangDay.Controllers
{
    public class PhanCongGDController : Controller
    {
        private readonly quanlygiaovienEntities _context = new quanlygiaovienEntities();

        // GET: PhanCongGD
        public ActionResult Index()
        {
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
                var data = _context.LopHocMonHoc
                            .Join(
                                _context.GiaoVien,
                                lhmh => lhmh.MaGV,
                                gv => gv.MaGV,
                                (lhmh, gv) => new { LHMH = lhmh, GV = gv })
                            .Join(
                                _context.MonHoc,
                                lhmh => lhmh.LHMH.MaMH,
                                mh => mh.MaMH,
                                (lhmh, mh) => new { LHMH = lhmh, MH = mh})
                            .Select (c => new
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

                if(!string.IsNullOrEmpty(lopHoc))
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

                var modifiedData = data.Select(x => new PhanCongGDModel()
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
            var lhmh = id != null ? _context.LopHocMonHoc.Find(id.Trim()) : new LopHocMonHoc();
            SelectList lophocs;
            if (id != "")
                lophocs = new SelectList(_context.LopHoc.ToList(), "MaLop", "TenLop", lhmh.MaLop);
            else
                lophocs = new SelectList(_context.LopHoc.ToList(), "MaLop", "TenLop");
            ViewData["LopDropdown"] = lophocs;

            SelectList monhocs;
            if (id != "")
                monhocs = new SelectList(_context.MonHoc.ToList(), "MaMH", "TenMon", lhmh.MaLop);
            else
                monhocs = new SelectList(_context.MonHoc.ToList(), "MaMH", "TenMon");
            ViewData["MonHocDropdown"] = monhocs;

            SelectList giaoviens;
            if (id != "")
                giaoviens = new SelectList(_context.GiaoVien.ToList(), "MaGV", "TenGV", lhmh.MaGV);
            else
                giaoviens = new SelectList(_context.GiaoVien.ToList(), "MaGV", "TenGV");
            ViewData["GiaoVienDropdown"] = giaoviens;

            SelectList giohocs;
            if (id != "")
                giohocs = new SelectList(_context.GioHoc.ToList(), "MaGio", "TenGio", lhmh.MaGio);
            else
                giohocs = new SelectList(_context.GioHoc.ToList(), "MaGio", "TenGio");
            ViewData["GioHocDropdown"] = giohocs;

            return PartialView("PhanCongGDPartialView", lhmh);
        }

        public ActionResult GetPhanCongGD()
        {
            SelectList lophocs = new SelectList(_context.LopHoc.ToList(), "MaLop", "TenLop");
            ViewData["LopDropdown"] = lophocs;

            SelectList monhocs = new SelectList(_context.MonHoc.ToList(), "MaMH", "TenMon");
            ViewData["MonHocDropdown"] = monhocs;

            SelectList giohocs = new SelectList(_context.GioHoc.ToList(), "MaGio", "TenGio");
            ViewData["GioHocDropdown"] = giohocs;

            return View("PhanCongGDView");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetGiaoVienByLopHocMonHoc(string lopHoc, string monHoc, string gioHoc, string ngayBD, string ngayKT)
        {
            var giaoviens = GetAllGiaoVienByLopMon(lopHoc, monHoc, gioHoc, ngayBD, ngayKT);
            return Json(giaoviens, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllGiaoVienByLopMon(string lopHoc, string monHoc, string gioHoc, string ngayBD, string ngayKT)
        {
            //generate empty list
            var selectList = new List<SelectListItem>();

            DateTime bd = Convert.ToDateTime(ngayBD);
            DateTime kt = Convert.ToDateTime(ngayKT);

            var listOfLHMH = _context.LopHocMonHoc.Where(x => x.MaLop.Trim().Equals(lopHoc.Trim()))
                .Where(x => x.MaMH.Trim().Equals(monHoc.Trim()))
                .Where(x => x.MaGio.Trim().Equals(gioHoc.Trim()))
                .Where(x => x.NgayBD >= bd)
                .Where(x => x.NgayBD <= kt)
                .Select(x => x.MaGV).ToList();

            var giaoViens = _context.GiaoVien.Where(r => !listOfLHMH.Contains(r.MaGV))
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
                    var isExist = _context.LopHocMonHoc.Where(x => x.MaLop == _obj.MaLop)
                       .Where(x => x.MaMH == _obj.MaMH)
                       .Where(x => x.MaGio == _obj.MaGio);
                    if (isExist != null)
                    {
                        return Json(new { Message = "E|Lớp học đã được phân công trước đó!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var countOfRows = _context.LopHocMonHoc.Count();
                        var lastRow = _context.LopHocMonHoc.OrderBy(c => c.MaLHMH).Skip(countOfRows - 1).FirstOrDefault();
                        int nextId = Convert.ToInt32(lastRow.MaLHMH.Substring(4));
                        _obj.MaLHMH = "LHMH" + (nextId + 1);
                        _context.LopHocMonHoc.Add(_obj);
                    }
                }
                _context.SaveChanges();
                return Json(new { Message = "S|Lưu dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = "E|Đã xãy ra lỗi trong quá trình lưu dữ liệu!" }, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult DeletePhanCongGD(string id)
        {
            try
            {
                var lhmh = _context.LopHocMonHoc.Find(id);
                _context.LopHocMonHoc.Remove(lhmh);
                _context.SaveChanges();
                return Json(new { Message = "Xóa dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Xảy ra lỗi trong quá trình xóa dữ liệu!" }, JsonRequestBehavior.AllowGet);
            }

        }
    }

    public class PhanCongGDModel
    {
        public string MaLHMH { get; set; }
        public string MaLop { get; set; }
        public string MaMH { get; set; }
        public string MaGio { get; set; }
        public string MoTa { get; set; }
        public string NgayKT { get; set; }
        public string NgayBD { get; set; }
        public string NgoaiGio { get; set; }
        public string MaGV { get; set; }
    }
}