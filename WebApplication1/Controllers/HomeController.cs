using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication1.Models;
using WebApplication1.Handler;

namespace WebApplication1.Controllers
{
    public class HomeModel
    {
        public List<BaiViet> posts;
        public user user;
    }
    public class HomeController : Controller
    {
        private tnut_socialEntities dtb = new tnut_socialEntities();

        public string GetPosts()
        {

            string username = Session["username"].ToString();
            string maNhom = Request.Form["maNhom"];
            int page_number = Int32.Parse(Request.Form["page_number"]);

            int? manhom;
            if (maNhom == "")
                manhom = null;
            else manhom = Int32.Parse(maNhom);
            HandlePost handelPost = new HandlePost(username, manhom, page_number);
            List<BaiViet> posts = handelPost.getPosts();
            Console.WriteLine(posts);
            string json = JsonConvert.SerializeObject(posts);
            return json;
        }

        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            string username = Session["username"].ToString();
            tnut_socialEntities db = new tnut_socialEntities();
            var user_info = db.user.Where(user => user.uid == username).FirstOrDefault();
            //ViewBag.user = user_info;
            //var ds_nhom = from dt_nhom in db.user_group where dt_nhom.uid == username 
            //              select dt_nhom.ma_nhom;
            ////var dt = db.user_group.Where(group => group.uid == username).ToList();
            //var data_nhom = ds_nhom.ToList();

            //var ds_post = from dt_post in db.post
            //              join dt_user in db.user on dt_post.uid equals dt_user.uid
            //              join dt_group in db.@group on dt_post.ma_nhom equals dt_group.ma_nhom
            //              where (data_nhom.Contains((int)dt_post.ma_nhom) && dt_post.status==true)
            //              orderby dt_post.ngay_dang descending
            //              select  new BaiViet{ 
            //                  id = dt_post.uid,
            //                  id_post = dt_post.id_post,
            //                  nd = dt_post.noi_dung,
            //                  nguoi_dang = dt_user.ho_ten,
            //                  ten_nhom = dt_group.ten_nhom,
            //                  time = (DateTime)dt_post.ngay_dang,
            //                  avatar = dt_user.anh,
            //                  ma_nhom = (int)dt_post.ma_nhom,
            //              };
            //ds_post = ds_post.Take(10);


            //var posts = ds_post.ToList();
            //Session["last_idpost"] = posts.LastOrDefault().id_post;

            //foreach (var post in posts)
            //{
            //    var timeSpan = DateTime.Now - post.time;
            //    post.timeSpan = (
            //        (timeSpan).Days > 0 ?
            //        (timeSpan).Days.ToString() + " ngày trước"
            //        :(
            //            (timeSpan).Hours.ToString()
            //            + " giờ "
            //            + (timeSpan).Minutes.ToString()
            //            + " phút trước."
            //        )
            //     );
            //    string a = post.nd;
            //    post.cmt = from cmt in db.comment
            //                join user in db.user on cmt.uid equals user.uid
            //                where cmt.id_post == post.id_post
            //                select new cmt
            //                {
            //                    id = cmt.id_comment,
            //                    uid = cmt.uid,
            //                    id_post = cmt.id_post,
            //                    nd = cmt.noi_dung,
            //                    ten = user.ho_ten,
            //                    avatar = user.anh
            //                };
            //    post.like = db.like.Where(like => like.id_post == post.id_post);
            //    post.img = db.anh.Where(img => img.id_post == post.id_post);
            //    var dt_user_like = db.like.Where(dt => dt.id_post == post.id_post && dt.uid == username).FirstOrDefault();
            //    if (dt_user_like != null) post.user_like = true;
            //    else post.user_like = false;
            //}
            //ViewBag.uid = username;
            return View(user_info);
        }
        
        public string DeletePost()
        {
            tnut_socialEntities db = new tnut_socialEntities();
            if (Session["username"] == null) return "Chưa đăng nhập";
            string username = Session["username"].ToString();
            int id_post = Int32.Parse(Request.Form["id_post"].ToString());
            var dt_post = db.post.Where(dt => dt.uid == username && dt.id_post == id_post).FirstOrDefault();
            if (dt_post == null) return "Không có quyền xóa bài viết!";
            db.post.Remove(dt_post);
            int n = db.SaveChanges();
            if (n > 0) return "1";
            else return "Đã có lỗi xảy ra. Vui lòng thử lại";
        }
        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("index");
        }
        [HttpPost]
        public ActionResult UploadAvatar(HttpPostedFileBase avatarUpload)
        {
            string username = Session["username"].ToString();
            string path = Path.Combine(Server.MapPath("~/asset/images/avatar"),
                           Path.GetFileName(avatarUpload.FileName));

            avatarUpload.SaveAs(path);
            tnut_socialEntities db = new tnut_socialEntities();
            var get_user_db = db.user.SingleOrDefault(user => user.uid == username);
            string old_image = get_user_db.anh;
            if(old_image!="no-image.png")
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/asset/images/avatar"), Path.GetFileName(old_image)));
            if (get_user_db != null)
            {
                get_user_db.anh = avatarUpload.FileName;
                int result = db.SaveChanges();
                
            }
            return RedirectToAction("Index");
        }
        public bool changepass()
        {
            if (Session["username"] == null) return false;
            string username = Session["username"].ToString();
            string old_pass = Request.Form["old_pass"];
            string new_pass = Request.Form["new_pass"];
            string new_pass_retype = Request.Form["new_pass_retype"];
            if (new_pass != new_pass_retype) return false;
            tnut_socialEntities db = new tnut_socialEntities();
            var uInfo = db.user.Where(user => user.uid == username).FirstOrDefault();

            bool isMatchPassword = Crypto.VerifyHashedPassword(uInfo.password, old_pass);
            if (!isMatchPassword) return false;
            uInfo.password =Crypto.HashPassword(new_pass) ;
            int n = db.SaveChanges();
            if (n > 0) return true;
            return false;
        }
        [HttpPost]
        public string UpdateLike()
        {
            if (Session["username"] == null) return "Chưa đăng nhập!";
            string username = Session["username"].ToString();
            int id_post = Int32.Parse(Request.Form["id_post"].ToString());
            var dt_like = dtb.like.Where(dt => dt.uid == username && dt.id_post == id_post).FirstOrDefault();
            string action = "like";
            if(dt_like == null)
            {
                like dataLike = new like
                {
                    uid = username,
                    id_post = id_post,
                    time = DateTime.Now
                };
                dtb.like.Add(dataLike); 
            }
            else
            {
                dtb.like.Remove(dt_like);
                action = "unlike";
            }
            int n = dtb.SaveChanges();
            if (n > 0)
            {
                var dt_like_post = dtb.like.Where(dt => dt.id_post == id_post);
                int likeCount = dt_like_post.Count();
                return action+ "-" + likeCount;
            }
            return "Có lỗi xảy ra, vui lòng thử lại!";
        }
        public string ReadMore()
        {
            int page = Int32.Parse(Request.Form["page_number"]);

            if (Session["username"] == null)
            {
                return "";
            }
            tnut_socialEntities db = new tnut_socialEntities();
            string username = Session["username"].ToString();
            var user_info = db.user.Where(user => user.uid == username).FirstOrDefault();
            ViewBag.user = user_info;
            var ds_nhom = from dt_nhom in db.user_group
                          where dt_nhom.uid == username
                          select dt_nhom.ma_nhom;
            //var dt = db.user_group.Where(group => group.uid == username).ToList();
            var data_nhom = ds_nhom.ToList();
            int last_idpost;
            if (Session["last_idpost"] == null)
                last_idpost = 0;
            else
                last_idpost = Int32.Parse( Session["last_idpost"].ToString());
            
            var ds_post = from dt_post in db.post
                          join dt_user in db.user on dt_post.uid equals dt_user.uid
                          join dt_group in db.@group on dt_post.ma_nhom equals dt_group.ma_nhom
                          where (data_nhom.Contains((int)dt_post.ma_nhom) 
                                  && dt_post.status == true 
                                  && dt_post.id_post < last_idpost
                          )
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
                          };
            if (ds_post == null) return "";
            ds_post = ds_post.Take(10);
            var posts = ds_post.ToList();
            if (posts.LastOrDefault() == null) return "";
            Session["last_idpost"] = posts.LastOrDefault().id_post;
            foreach (var post in posts)
            {

                string a = post.nd;
                //post.cmt = db.comment.Where(cmt => cmt.id_post == post.id_post);
                var x = (from cmt in db.comment
                           join user in db.user on cmt.uid equals user.uid
                           where cmt.id_post == post.id_post
                           select new CustomCmt
                           {
                               id = cmt.id_comment,
                               uid = cmt.uid,
                               id_post = cmt.id_post,
                               nd = cmt.noi_dung,
                               ten = user.ho_ten,
                               avatar = user.anh
                           }).ToList();
                post.cmt = x;
                post.like = (from like in db.like
                             where like.id_post == post.id_post
                             select new CustomLike
                             {
                                 uid = like.uid,
                                 id_post = like.id_post
                             }).ToList();
                post.img = (from imgs in db.anh
                             where imgs.id_post == post.id_post
                             select new CustomAnh
                             {
                                 id_anh = imgs.id_image,
                                 id_post = (int)imgs.id_post,
                                 link = imgs.link

                             }).ToList();

                var dt_user_like = db.like.Where(dt => dt.id_post == post.id_post && dt.uid == username).FirstOrDefault();
                if (dt_user_like != null) post.user_like = true;
                else post.user_like = false;
            }
            string json = JsonConvert.SerializeObject(posts);
            return json;
        }
        public string AddComment()
        {
            if(Session["username"]== null) { return "Chưa đăng nhập nhá"; }
            string username = Session["username"].ToString();
            int id_post = Int32.Parse( Request.Form["id_post"]);
            string nd_comment = Request.Form["nd_comment"];
            if (nd_comment == "") return "Nội dung comment không được trống";
            comment new_cmt = new comment
            {
                id_post = id_post,
                uid = username,
                noi_dung = nd_comment,
                time = DateTime.Now,
                id_parent = null
            };
            dtb.comment.Add(new_cmt);
            int n = dtb.SaveChanges();
            string ten = dtb.user.Where(dt => dt.uid == username).FirstOrDefault().ho_ten;
            string avatar = dtb.user.Where(dt => dt.uid == username).FirstOrDefault().anh;
            if (n > 0)
            {
                int sl_comment = dtb.comment.Where(cmt => cmt.id_post == id_post).Count();
                string json = "{\"status\":\"success\", \"sl_comment\":\"" + sl_comment + "\", \"ten\":\"" + ten + "\", \"avatar\":\"" + avatar + "\"}";
                return json;

            }
            return "Có lỗi xảy ra, vui lòng thử lại!";
        }
    }
}