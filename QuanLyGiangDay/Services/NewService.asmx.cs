using Newtonsoft.Json;
using QuanLyGiangDay.Models.EF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace QuanLyGiangDay.Services
{
    /// <summary>
    /// Summary description for NewService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NewService : System.Web.Services.WebService
    {
        private readonly quanlygiaovienEntities _context = new quanlygiaovienEntities();

        [WebMethod(enableSession: true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void BindSchedule(string giaoVien, string ngayBD, string ngayKT)
        {
            DateTime bd = ngayBD == "" ? DateTime.MinValue : Convert.ToDateTime(ngayBD);
            DateTime kt = ngayKT == "" ? DateTime.MaxValue : Convert.ToDateTime(ngayKT);
            //Get data LophocMonHoc
            var data = _context.LopHocMonHoc
                .Where(x => x.NgayBD >= bd)
                .Where(x => x.NgayBD <= kt).ToList();


            List<string> colors = new List<string>();
            var random = new Random(); // Make sure this is out of the loop!
            for (int i = 0; i < data.Count; i++)
            {
                colors.Add(String.Format("#{0:X6}", random.Next(0x1000000)));
            }

            List<object> lstResult = new List<object>();
            string title = "";
            if (giaoVien != "") {
                for (int i = 0; i < data.Count; i++)
                {
                    var item = data.ElementAt(i);
                    if (item.MaGV != null && item.MaGV.Trim().Equals(giaoVien.Trim()))
                    {
                        string a = item.NgayBD.Value.ToString("dd-MM-yyyy");
                        if (item.NgayBD == null || item.NgayKT == null)
                            title = "Chưa có dữ liệu phân công";
                        else
                        {
                            title = item.GiaoVien != null ? item.GiaoVien.TenGV : "Chưa có dữ liệu Giáo viên";
                            title += " - ";
                            title += item.MonHoc != null ? item.MonHoc.TenMon : "Chưa có dữ liệu Môn học";
                            title += " - ";
                            title += item.MonHoc != null ? item.MonHoc.LopHocMonHoc.ElementAt(0).LopHoc.MaLop : "Chưa có dữ liệu Lớp học";
                            title += " - ";
                            title += "Từ ngày " + item.NgayBD.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) 
                                + " Đến ngày " + item.NgayKT.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        TestObject schedule = new TestObject
                        {
                            id = i.ToString(),
                            title = title,
                            description = item.MonHoc.TenMon,
                            start = item.NgayBD != null ? item.NgayBD.Value.ToString("yyyy-MM-dd") : "",
                            backgroundColor = colors.ElementAt(i % 10),
                            borderColor = colors.ElementAt(i % 10)
                        };
                        lstResult.Add(schedule);
                    }
                }
            } 
            else
            {
                for (int i = 0; i < data.Count; i++)
                {
                    var item = data.ElementAt(i);
                    if (item.NgayBD == null || item.NgayKT == null)
                        title = "Chưa có dữ liệu phân công";
                    else
                    {
                        title = item.GiaoVien != null ? item.GiaoVien.TenGV : "Chưa có dữ liệu Giáo viên";
                        title += " - ";
                        title += item.MonHoc != null ? item.MonHoc.TenMon : "Chưa có dữ liệu Môn học";
                        title += " - ";
                        title += item.MonHoc != null ? item.MonHoc.LopHocMonHoc.ElementAt(0).LopHoc.MaLop : "Chưa có dữ liệu Lớp học";
                        title += " - ";
                        title += "Từ ngày " + item.NgayBD.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                                + " Đến ngày " + item.NgayKT.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    TestObject schedule = new TestObject
                    {
                        id = i.ToString(),
                        title = title,
                        description = item.MonHoc.TenMon,
                        start = item.NgayBD != null ? item.NgayBD.Value.ToString("yyyy-MM-dd") : "",
                        backgroundColor = colors.ElementAt(i % 10),
                        borderColor = colors.ElementAt(i % 10)
                    };
                    lstResult.Add(schedule);
                }
            }
            string JSONresult = string.Empty;
            JSONresult = JsonConvert.SerializeObject(lstResult);
            Context.Response.Write(JSONresult);
        }
    }

    public class TestObject
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string groupId { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
    }
}
