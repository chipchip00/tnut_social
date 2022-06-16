using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProfileController : Controller
    {
        private tnut_socialEntities db = new tnut_socialEntities();
        // GET: Profile
        public ActionResult Index(string id)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login", new { returnUrl = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + "/profile" });

            string username = Session["username"].ToString();
            if (id == null) id = username;
            try
            {
                var uinfo = db.user.Where(user => user.uid == id).FirstOrDefault();
                string nganh = (from dt_nganh in db.nganh
                                join dt_lop in db.lop on dt_nganh.ma_nganh equals dt_lop.ma_nganh
                                join dt_user in db.user on dt_lop.ma_lop equals dt_user.ma_lop
                                where dt_user.uid == id
                                select dt_nganh.ten_nganh).FirstOrDefault();
                ViewBag.nganh = nganh;
                if (id == username ||id==null) ViewBag.isMyProfile = "1";
                return View(uinfo);
            }
            catch(Exception e)
            {
                ViewBag.err = e.Message;
                return View();
            }
        }
        public ActionResult Edit()
        {
            if (Session["username"] == null) return RedirectToAction("Index","Login");
            string username = Session["username"].ToString();
            var uinfo = db.user.Where(dt => dt.uid == username).FirstOrDefault();
            return View(uinfo);
        }
        [HttpPost]
        public ActionResult Edit(user uinfo)
        {
            if (Session["username"] == null) return RedirectToAction("Index", "Login");
            string username = Session["username"].ToString();
            var new_description = uinfo.gioi_thieu;
            var dt_user = db.user.Where(dt => dt.uid == username).FirstOrDefault();
            dt_user.gioi_thieu = new_description;
            dt_user.dia_chi = uinfo.dia_chi;
            int n= db.SaveChanges();
            if (n > 0) return RedirectToAction("Index");
            ViewBag.error_message = "Có lỗi xảy ra. Vui lòng thử lại";
            return View();
        }
    }
}