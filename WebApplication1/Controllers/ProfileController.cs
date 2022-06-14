using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login", new { returnUrl = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + "/profile" });
            return View();
        }
    }
}