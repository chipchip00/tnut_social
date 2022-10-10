using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TaiLieuController : Controller
    {
        // GET: TaiLieu
        tnut_socialEntities db = new tnut_socialEntities();
        public ActionResult Index()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            var monhoc = db.mon_hoc;
            return View(monhoc);
        }
        public ActionResult ChiTiet(int mmh)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
           // if (mmh == null) return RedirectToAction("Index");
            var tailieu = db.tai_lieu.Where(dt => dt.ma_mon_hoc == mmh);
            return View(tailieu);
        }
    }
}