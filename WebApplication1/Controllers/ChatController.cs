using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index(string id_nguoi_nhan="")
        {
            if (Session["username"]== null)
            {
                return RedirectToAction("Index", "Login", new { returnUrl = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + "/chat" });
            }
            tnut_socialEntities db = new tnut_socialEntities();
            string username = Session["username"].ToString();
            //var list_nguoi_nhan = db.chat.Where(dt => dt.uid == username).Select(dt => dt.nguoi_nhan).ToList();
            //id_nguoi_nhan = list_nguoi_nhan[0];

            var db_nguoi_nhan = (from dt_user in db.user
                                 from dt_chat in db.chat
                                where ((dt_chat.uid == username || dt_chat.nguoi_nhan == username)
                                     && (dt_chat.uid ==dt_user.uid ||dt_chat.nguoi_nhan ==dt_user.uid)
                                )
                                select dt_user).Distinct();
            
            var data = db_nguoi_nhan.FirstOrDefault();
            if (id_nguoi_nhan == "") id_nguoi_nhan = data.uid;
            Session["default_reciver"] = id_nguoi_nhan;
            var chat_db = db.chat.Where(dt => (dt.uid == username && dt.nguoi_nhan== id_nguoi_nhan)||(dt.uid == id_nguoi_nhan && dt.nguoi_nhan == username)).ToList();
            var recievers = db_nguoi_nhan.ToList();
            ViewBag.nguoi_nhan = recievers.Where(user => user.uid == id_nguoi_nhan).FirstOrDefault();
            ViewBag.list_nguoi_nhan = recievers;
            ViewBag.nd_chat = chat_db;
            return View(db);
        }
        [HttpPost]
        public Boolean Save(string id_nguoi_nhan, string message)
        {
            if (Session["username"] == null) return false;
            if (id_nguoi_nhan == null|| id_nguoi_nhan =="") id_nguoi_nhan = Session["default_reciver"].ToString();
            tnut_socialEntities db = new tnut_socialEntities();
            var dt_chat = new chat()
            {
                uid = Session["username"].ToString(),
                nguoi_nhan = id_nguoi_nhan,
                noi_dung = message,
                time = DateTime.Now
            };
            db.chat.Add(dt_chat);
            int n= db.SaveChanges();
            if (n > 0) return true;
            return false;
        }

    }
}