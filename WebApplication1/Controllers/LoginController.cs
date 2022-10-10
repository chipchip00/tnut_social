using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
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
        public ActionResult Index(string username, string password )
        {
            if(username =="" || password == "")
            {
                ViewBag.error_message = "Tên đăng nhập hoặc mật khẩu không được để trống!";
                return View();
            }
            tnut_socialEntities db = new tnut_socialEntities();
            var userData = db.user.Where(user => user.uid == username);
            if (userData.Count() > 0)
            {
                string hashedPassword = userData.FirstOrDefault().password;
                bool isCorrect = Crypto.VerifyHashedPassword(hashedPassword, password);
                if (isCorrect)
                {
                    string uid = userData.Select(dt => dt.uid).FirstOrDefault().ToString();
                    string ten = userData.Select(dt => dt.ho_ten).FirstOrDefault().ToString();
                    string returnUrl = Request.QueryString["returnUrl"];
                    Session["username"] = uid;
                    Session["name"] = ten;
                    return RedirectToLocal(returnUrl);
                    //return RedirectToAction("Index", "Home");
                }

            }
            ViewBag.error_message = "Tên đăng nhập hoặc mật khẩu không đúng, vui lòng thử lại!";
            return View();
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (returnUrl != "" && returnUrl!=null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult forgot()
        {
            return View();
        }
        private static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [HttpPost]
        public ActionResult SendEmail(user uinfo )
        {
            string receiver = uinfo.email;
            string new_pass = RandomString(8);
            MailAddress addr = new MailAddress(receiver);
            string username = addr.User;
            string domain = addr.Host;
            if (domain != "tnut.edu.vn")
            {
                TempData["Error"] = "Email không đúng định dạng, vui lòng thử lại!";
                return RedirectToAction("forgot");
            }
            tnut_socialEntities db = new tnut_socialEntities();
            var uInfo = db.user.Where(user => user.uid == username).FirstOrDefault();
            var hashedPassword = Crypto.HashPassword(new_pass);
            uInfo.password = hashedPassword;

            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("chipchjp2002@gmail.com", "TNUT Social");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "zkejlbjjnqlyumyy";
                    var subject = "Email cấp phát lại mật khẩu";
                    var body = "Mật khẩu mới của bạn là: "+new_pass;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                        int n = db.SaveChanges();
                        if (n <= 0) return RedirectToAction("forgot");
                    }
                    return RedirectToAction("Index");
                }

        }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ViewBag.Error = "Có lỗi xảy ra, vui lòng thử lại!";
            }
            return RedirectToAction("forgot");
        }
    }
}