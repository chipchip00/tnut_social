using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class DSNganh
    {
        public int ma_nganh;
        public string ten_nganh;
    }
    public class DSLop
    {
        public string ma_lop;
        public string ten_lop;
    }
    public class SearchController : Controller
    {
        tnut_socialEntities db = new tnut_socialEntities();
        // GET: Search
        public ActionResult Index(string searchString="")
        {
            if (Session["username"] == null) return RedirectToAction("Index", "Login");
            var dt_khoa = db.khoa;
            var dt_nganh = db.nganh.Where(dt=>dt.ma_khoa == dt_khoa.FirstOrDefault().ma_khoa);
            var dt_lop = db.lop.Where(dt => dt.ma_nganh == dt_nganh.FirstOrDefault().ma_nganh);
            var dt_user = db.user.Where(dt => dt.ho_ten.Contains(searchString) || dt.gioi_thieu.Contains(searchString) || dt.dia_chi.Contains(searchString));
            var users = from user in db.user
                        join lop in db.lop on user.ma_lop equals lop.ma_lop
                        join nganh in db.nganh on lop.ma_nganh equals nganh.ma_nganh
                        join dat_khoa in db.khoa on nganh.ma_khoa equals dat_khoa.ma_khoa
                        where (user.ho_ten.Contains(searchString) || user.gioi_thieu.Contains(searchString) || user.dia_chi.Contains(searchString))
                        select new UserInfo
                        {
                            uid = user.uid,
                            ho_ten = user.ho_ten,
                            email = user.email,
                            dia_chi = user.dia_chi,
                            maLop = user.ma_lop,
                            role = user.role,
                            anh = user.anh,
                            gioi_thieu = user.gioi_thieu,
                            gioi_tinh = user.gioi_tinh,
                            maNganh = nganh.ma_nganh,
                            maKhoa = dat_khoa.ma_khoa,
                            tenLop = lop.ten_lop,
                            tenNganh = nganh.ten_nganh,
                            tenKhoa = dat_khoa.ten_khoa

                        };

            ViewBag.searchString = searchString;
            ViewBag.dt_khoa = dt_khoa;
            ViewBag.dt_nganh = dt_nganh;
            ViewBag.dt_lop = dt_lop;
            return View(users);
        }
        public string qKhoa()
        {
            try
            {
                int maKhoa = Int32.Parse(Request.Form["khoa"].ToString());
                var dsNganh = from nganh in db.nganh
                              where nganh.ma_khoa == maKhoa
                              select new DSNganh
                              {
                                  ma_nganh = nganh.ma_nganh,
                                  ten_nganh = nganh.ten_nganh
                              };
                string Json = Newtonsoft.Json.JsonConvert.SerializeObject(dsNganh);
                return Json;
            }
            catch
            {
                return "";
            }

        }
        public string qNganh()
        {
            try
            {
                int maNganh = Int32.Parse(Request.Form["nganh"].ToString());
                var dsLop = from lop in db.lop
                            where lop.ma_nganh == maNganh
                            select new DSLop
                            {
                                ma_lop = lop.ma_lop,
                                ten_lop = lop.ten_lop
                            };
                string Json = Newtonsoft.Json.JsonConvert.SerializeObject(dsLop);
                return Json;
            }
            catch
            {
                return "";
            }
        }
        public string reloadSearch()
        {
            try
            {

                string ma_lop = Request.Form["maLop"];
                string ma_nganh = Request.Form["ma_nganh"];
                string ma_khoa = Request.Form["ma_khoa"];
                bool gioi_tinh;
                if (Request.Form["gioi_tinh"] == "1") gioi_tinh = true;
                else gioi_tinh = false;
                string role = Request.Form["role"];
                string searchString = Request.Form["searchString"];

                var users = from user in db.user
                            join lop in db.lop on user.ma_lop equals lop.ma_lop
                            join nganh in db.nganh on lop.ma_nganh equals nganh.ma_nganh
                            join dat_khoa in db.khoa on nganh.ma_khoa equals dat_khoa.ma_khoa
                            where ((user.ho_ten.Contains(searchString) || user.gioi_thieu.Contains(searchString) || user.dia_chi.Contains(searchString))
                                    && user.gioi_tinh == gioi_tinh && user.role == role
                            )
                            select new UserInfo
                            {
                                uid = user.uid,
                                ho_ten = user.ho_ten,
                                email = user.email,
                                dia_chi = user.dia_chi,
                                maLop = user.ma_lop,
                                role = user.role,
                                anh = user.anh,
                                gioi_thieu = user.gioi_thieu,
                                gioi_tinh = user.gioi_tinh,
                                maNganh = nganh.ma_nganh,
                                maKhoa = dat_khoa.ma_khoa,
                                tenLop = lop.ten_lop,
                                tenNganh = nganh.ten_nganh,
                                tenKhoa = dat_khoa.ten_khoa

                            };
                if (ma_khoa == "all" && ma_nganh == "all" && ma_lop == "all")
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(users);
                }
                else if (ma_khoa != "all" && ma_nganh != "all" && ma_lop != "all")
                {
                    int ma_nganh_int = Int32.Parse(ma_nganh);
                    int ma_khoa_int = Int32.Parse(ma_khoa);

                    var users3 = users.Where(dt => dt.ma_lop == ma_lop && dt.maNganh == ma_nganh_int && dt.maKhoa == ma_khoa_int);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(users3);
                }
                else
                {
                    if (ma_nganh == "all" && ma_lop == "all")
                    {
                        int ma_khoa_int = Int32.Parse(ma_khoa);
                        var users1 = users.Where(dt => dt.maKhoa == ma_khoa_int);
                        return Newtonsoft.Json.JsonConvert.SerializeObject(users1);
                    }
                    else if (ma_nganh == "all" && ma_khoa == "all")
                    {
                        var users4 = users.Where(dt => dt.maLop == ma_lop);
                        return Newtonsoft.Json.JsonConvert.SerializeObject(users4);
                    }
                    else if (ma_khoa == "all" && ma_lop == "all")
                    {
                        int ma_nganh_int = Int32.Parse(ma_nganh);
                        var users2 = users.Where(dt => dt.maNganh == ma_nganh_int);
                        return Newtonsoft.Json.JsonConvert.SerializeObject(users2);

                    }
                    else
                    {
                        if (ma_khoa != "all" && ma_nganh != "all")
                        {
                            int ma_nganh_int = Int32.Parse(ma_nganh);
                            int ma_khoa_int = Int32.Parse(ma_khoa);
                            var users5 = users.Where(dt => dt.maNganh == ma_nganh_int && dt.maKhoa == ma_khoa_int);
                            return Newtonsoft.Json.JsonConvert.SerializeObject(users5);
                        }
                        else if (ma_khoa != "all" && ma_lop != "all")
                        {
                            int ma_khoa_int = Int32.Parse(ma_khoa);
                            var users6 = users.Where(dt => dt.maLop == ma_lop && dt.maKhoa == ma_khoa_int);
                            return Newtonsoft.Json.JsonConvert.SerializeObject(users6);
                        }
                        else
                        {
                            int ma_nganh_int = Int32.Parse(ma_nganh);
                            var users7 = users.Where(dt => dt.maLop == ma_lop && dt.maNganh == ma_nganh_int);
                            return Newtonsoft.Json.JsonConvert.SerializeObject(users7);
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
            
        }
    }
}