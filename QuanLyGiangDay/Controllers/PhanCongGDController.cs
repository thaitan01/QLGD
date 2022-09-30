using QuanLyGiangDay.Models.EF;
using System;
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
        public ActionResult GetPhanCongGDTableData()
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
                                (lhmh, mh) => new { LHMH = lhmh, MH = mh})
                            .Select (c => new
                            {
                                c.LHMH.LHMH.MaLHMH,
                                c.LHMH.LHMH.MaLop,
                                c.MH.TenMon,
                                c.LHMH.LHMH.MaGio,
                                c.LHMH.LHMH.MoTa,
                                c.LHMH.LHMH.NgayDayDuKien,
                                c.LHMH.LHMH.NgayBD,
                                c.LHMH.LHMH.NgoaiGio,
                                c.LHMH.GV.TenGV
                            }).ToList();
                            
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
                            data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NgayDayDuKien).ToList()
                                                                                                       : data.OrderBy(p => p.NgayDayDuKien).ToList();
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
                    NgayDayDuKien = x.NgayDayDuKien.Value.ToString("d/M/yyyy"),
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
    }

    public class PhanCongGDModel
    {
        public string MaLHMH { get; set; }
        public string MaLop { get; set; }
        public string MaMH { get; set; }
        public string MaGio { get; set; }
        public string MoTa { get; set; }
        public string NgayDayDuKien { get; set; }
        public string NgayBD { get; set; }
        public string NgoaiGio { get; set; }
        public string MaGV { get; set; }
    }
}