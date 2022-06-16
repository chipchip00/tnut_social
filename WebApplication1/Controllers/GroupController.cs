using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                //TempData["returnUrl"] = Server.MapPath("/") + "group";
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
            if(Session["username"]== null)
            {
                
                return RedirectToAction("Index","Login");
            }
            if (ma_nhom == null) return RedirectToAction("index");
            string username = Session["username"].ToString();
            
            tnut_socialEntities db = new tnut_socialEntities();
            var user_gr = db.user_group.Where(ug => ug.ma_nhom == ma_nhom && ug.uid == username).FirstOrDefault();
            if (user_gr == null) return RedirectToAction("Index");
            var user_info = db.user.Where(user => user.uid == username).FirstOrDefault();
            ViewBag.user = user_info;


            var ds_post = from dt_post in db.post
                          join dt_user in db.user on dt_post.uid equals dt_user.uid
                          join dt_group in db.@group on dt_post.ma_nhom equals dt_group.ma_nhom
                          where (dt_group.ma_nhom==ma_nhom && dt_post.status == true)
                          orderby dt_post.ngay_dang descending
                          select new BaiViet
                          {
                              id = dt_post.uid,
                              id_post = dt_post.id_post,
                              nd = dt_post.noi_dung,
                              nguoi_dang = dt_user.ho_ten,
                              ten_nhom = dt_group.ten_nhom,
                              time = (DateTime)dt_post.ngay_dang,
                              avatar = dt_user.anh,
                              ma_nhom = (int)dt_post.ma_nhom
                              //cmt = dt_comment,
                              //img = dt_image,
                              //like = dt_like
                          };



            var posts = ds_post.ToList();
            //Session["last_idpost_group"] = posts.LastOrDefault().id_post;
            foreach (var post in posts)
            {

                string a = post.nd;
                //post.cmt = db.comment.Where(cmt => cmt.id_post == post.id_post);
                post.cmt = from cmt in db.comment
                           join user in db.user on cmt.uid equals user.uid
                           where cmt.id_post == post.id_post
                           select new cmt
                           {
                               id = cmt.id_comment,
                               uid = cmt.uid,
                               id_post = cmt.id_post,
                               nd = cmt.noi_dung,
                               ten = user.ho_ten,
                               avatar = user.anh
                           };
                post.like = db.like.Where(like => like.id_post == post.id_post);
                post.img = db.anh.Where(img => img.id_post == post.id_post);
                var dt_user_like = db.like.Where(dt => dt.id_post == post.id_post && dt.uid == username).FirstOrDefault();
                if (dt_user_like != null) post.user_like = true;
                else post.user_like = false;
            }
            ViewBag.list_user = (from user in db.user
                                join u_group in db.user_group on user.uid equals u_group.uid
                                where u_group.ma_nhom == ma_nhom
                                select user).ToList();
            ViewBag.role = db.user_group.Where(dt => dt.uid == username && dt.ma_nhom == ma_nhom).FirstOrDefault().id_role;
            ViewBag.nhom = db.group.Where(gr => gr.ma_nhom == ma_nhom).FirstOrDefault();

            return View(posts);
        }

        [HttpPost]
        public ActionResult AddPost(HttpPostedFileBase[] uploadFile, string noi_dung_post, int get_id_nhom)
        {
            string username = Session["username"].ToString();
            tnut_socialEntities db = new tnut_socialEntities();
            var roleInGroup = db.user_group.Where(dt => dt.uid == username && dt.ma_nhom == get_id_nhom).FirstOrDefault().id_role;
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
            if (ModelState.IsValid)
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
                            string path = Path.Combine(Server.MapPath("~/asset/images/post"),
                                                       Path.GetFileName(file.FileName));

                            file.SaveAs(path);
                            ViewBag.Message = "File uploaded successfully";
                           
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "You have not specified a file.";
                    }
                }
                
            }
            return RedirectToAction("ChiTiet", new { ma_nhom = get_id_nhom });
        }
        
        public ActionResult KiemDuyet(int ?ma_nhom)
        {
            if (Session["username"] == null) return RedirectToAction("Index", "login");
            tnut_socialEntities db = new tnut_socialEntities();
            string username = Session["username"].ToString();
            if (ma_nhom == null) return RedirectToAction("Index");
            var roleInGroup = db.user_group.Where(ug => ug.uid == username && ug.ma_nhom == ma_nhom).First().id_role;
            if (roleInGroup != 1) return RedirectToAction("Index");
            //ViewBag.nhom = db.group.Where(dt => dt.ma_nhom == ma_nhom).FirstOrDefault();
            
            var baiVietCho = db.post.Where(post => post.ma_nhom == ma_nhom && post.status == false);
            var ds_post = from dt_post in db.post
                          join dt_user in db.user on dt_post.uid equals dt_user.uid
                          join dt_group in db.@group on dt_post.ma_nhom equals dt_group.ma_nhom
                          //join dt_like in db.like on dt_post.id_post equals dt_like.id_post
                          //join dt_comment in db.comment on dt_post.id_post equals dt_comment.id_post
                          //join dt_image in db.anh on dt_post.id_post equals dt_image.id_post
                          where (dt_post.ma_nhom == ma_nhom && dt_post.status == false)
                          select new BaiViet
                          {
                              id = dt_post.uid,
                              id_post = dt_post.id_post,
                              nd = dt_post.noi_dung,
                              nguoi_dang = dt_user.ho_ten,
                              ten_nhom = dt_group.ten_nhom,
                              time = (DateTime)dt_post.ngay_dang,
                              avatar = dt_user.anh,
                              ma_nhom = (int)dt_post.ma_nhom
                              //cmt = dt_comment,
                              //img = dt_image,
                              //like = dt_like
                          };

            var posts = ds_post.ToList();
            foreach (var post in posts)
            {
                string a = post.nd;
                //post.cmt = db.comment.Where(cmt => cmt.id_post == post.id_post);
                post.cmt = from cmt in db.comment
                           join user in db.user on cmt.uid equals user.uid
                           where cmt.id_post == post.id_post
                           select new cmt
                           {
                               id = cmt.id_comment,
                               uid = cmt.uid,
                               id_post = cmt.id_post,
                               nd = cmt.noi_dung,
                               ten = user.ho_ten,
                               avatar = user.anh
                           };
                post.like = db.like.Where(like => like.id_post == post.id_post);
                post.img = db.anh.Where(img => img.id_post == post.id_post);
                var dt_user_like = db.like.Where(dt => dt.id_post == post.id_post && dt.uid == username).FirstOrDefault();
                if (dt_user_like != null) post.user_like = true;
                else post.user_like = false;
            }
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