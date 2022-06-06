using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            tnut_socialEntities db = new tnut_socialEntities();
            var data = db.user.Where(user => user.uid == username && user.password == password);
            
            int numberOfData = data.Count();
            if (numberOfData > 0)
            {
                string uid = data.Select(dt => dt.uid).FirstOrDefault().ToString();
                string ten = data.Select(dt => dt.ho_ten).FirstOrDefault().ToString();

                Session["username"] = uid;
                Session["name"] = ten;
                return RedirectToAction("Index", "Chat");
            }
            else ViewBag.error_message = "Đăng nhập thất bại, vui lòng thử lại!";
            return View();
        }
    }
}