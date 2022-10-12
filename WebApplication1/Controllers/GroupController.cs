using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Handler;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GroupController : Controller
    {
        // GET: Group
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("index", "login", new { returnUrl= HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + "/group" });
            }
            tnut_socialEntities db = new tnut_socialEntities();
            string username = Session["username"].ToString();
            var groups = from gr in db.@group
                         join u_group in db.user_group on gr.ma_nhom equals u_group.ma_nhom
                         where u_group.uid == username
                         select gr;
            return View(groups);
        }
        public ActionResult ChiTiet(int ?ma_nhom)
        {
            if(Session["username"] == null)
            {
                return RedirectToAction("Index","Login");
            }
            if (ma_nhom == null) return RedirectToAction("index");
            string username = Session["username"].ToString();
            
            tnut_socialEntities db = new tnut_socialEntities();
            var user_gr = db.user_group.Where(ug => ug.ma_nhom == ma_nhom && ug.uid == username).FirstOrDefault();
            if (user_gr == null) return RedirectToAction("Index");
            var list_user = (from user in db.user
                                join u_group in db.user_group on user.uid equals u_group.uid
                                where u_group.ma_nhom == ma_nhom
                                select user).ToList();
            ViewBag.role = db.user_group.Where(
                dt => dt.uid == username && dt.ma_nhom == ma_nhom
                ).FirstOrDefault().id_role;
            ViewBag.nhom = db.group.Where(gr => gr.ma_nhom == ma_nhom).FirstOrDefault();

            return View(list_user);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddPost(HttpPostedFileBase[] uploadFile, string noi_dung_post, int get_id_nhom)
        {
            
            if(uploadFile.Length== 0 && noi_dung_post == "")
            {
                TempData["publish_post_result"] = "Lỗi: Nội dung trống.";
                return RedirectToAction("ChiTiet", new { ma_nhom = get_id_nhom });
            }
            string username = Session["username"].ToString();
            tnut_socialEntities db = new tnut_socialEntities();
            var roleInGroup = db.user_group.Where(
                dt => dt.uid == username 
                && dt.ma_nhom == get_id_nhom
                ).FirstOrDefault().id_role;
            bool status = false;
            if (roleInGroup == 1) status = true;
            var new_post = new post
            {
                noi_dung = noi_dung_post,
                uid = username,
                ma_nhom = get_id_nhom,
                ngay_dang = DateTime.Now,
                id_category = 1,
                status = status
            };
            db.post.Add(new_post);
            db.SaveChanges();
            int abc = uploadFile.Length;
            int def = uploadFile.Count();
            if (ModelState.IsValid && uploadFile.Length > 0)
            {   //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in uploadFile)
                    {
                    //Checking file is available to save.  
                    if (file != null && file.ContentLength > 0)
                    {
                        try
                        {
                            tnut_socialEntities dt = new tnut_socialEntities();
                            var new_anh = new anh
                            {
                                id_post = new_post.id_post,
                                link = file.FileName,
                                mo_ta = "No",

                            };
                            dt.anh.Add(new_anh);
                            dt.SaveChanges();
                            var filePath = Server.MapPath(string.Format("~/asset/images/post/{0}", new_post.id_post));
                            bool isExistsPath = System.IO.Directory.Exists(filePath);

                            if (!isExistsPath)
                                System.IO.Directory.CreateDirectory(filePath);
                            string path = Path.Combine(filePath,
                                                       Path.GetFileName(file.FileName));

                            file.SaveAs(path);
                        }
                        catch (Exception ex)
                        {
                            TempData["publish_post_result"] = "Lỗi:" + ex.Message.ToString();
                            return RedirectToAction("ChiTiet", new { ma_nhom = get_id_nhom });
                        }
                    }
                    else
                    {
                        TempData["publish_post_result"] = "File upload không hợp lệ!";
                        return RedirectToAction("ChiTiet", new { ma_nhom = get_id_nhom });
                    }
                }
                TempData["publish_post_result"] = "ok";
                db.SaveChanges();

            }
            return RedirectToAction("ChiTiet", new { ma_nhom = get_id_nhom });
        }
        
        public ActionResult KiemDuyet(int ?ma_nhom, int pageNumber = 0)
        {
            if (Session["username"] == null) return RedirectToAction("Index", "login");
            tnut_socialEntities db = new tnut_socialEntities();
            string username = Session["username"].ToString();
            if (ma_nhom == null) return RedirectToAction("Index");
            var roleInGroup = db.user_group.Where(ug => ug.uid == username && ug.ma_nhom == ma_nhom).First().id_role;
            if (roleInGroup != 1) return RedirectToAction("Index");

            HandlePost handlePost = new HandlePost(username, ma_nhom, pageNumber);
            handlePost.status = false;
            var posts = handlePost.getPosts();
            ViewBag.pageNumber = pageNumber;
            ViewBag.maNhom = ma_nhom;
            return View(posts);
        }
        public string DuyetPost()
        {
            tnut_socialEntities db = new tnut_socialEntities();
            if (Session["username"] == null) return "Chưa đăng nhập nha";
            string username = Session["username"].ToString();
            int id_post = Int32.Parse(Request.Form["id_post"]);
            string action = Request.Form["action"];
            int id_group = (int)db.post.Where(dt => dt.id_post == id_post).FirstOrDefault().ma_nhom;
            int roleInGroup = (int)db.user_group.Where(dt => dt.ma_nhom == id_group && dt.uid == username).First().id_role;
            if (roleInGroup != 1) return "Bạn không phải là quản trị viên";
            if (action == "approve")
            {
                var dt_post = db.post.Where(dt => dt.id_post == id_post).First();
                dt_post.status = true;
            }
            else
            {
                var dt_post = db.post.Where(dt => dt.id_post == id_post).First();
                db.post.Remove(dt_post);
            }
            int n = db.SaveChanges();
            if (n > 0) return "1";
            return "Có lỗi xảy ra, vui lòng thử lại!";
        }
    }
}