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
            var ds_post = from dt_post in db.post
                          join dt_user in db.user on dt_post.uid equals dt_user.uid
                          join dt_group in db.@group on dt_post.ma_nhom equals dt_group.ma_nhom
                          //join dt_like in db.like on dt_post.id_post equals dt_like.id_post
                          //join dt_comment in db.comment on dt_post.id_post equals dt_comment.id_post
                          //join dt_image in db.anh on dt_post.id_post equals dt_image.id_post
                          where (dt_post.ma_nhom == ma_nhom)
                          orderby dt_post.ngay_dang descending
                          select new BaiViet
                          {
                              id = dt_post.uid,
                              id_post = dt_post.id_post,
                              nd = dt_post.noi_dung,
                              nguoi_dang = dt_user.ho_ten,
                              ten_nhom = dt_group.ten_nhom,
                              time = (DateTime)dt_post.ngay_dang
                              //cmt = dt_comment,
                              //img = dt_image,
                              //like = dt_like
                          };

            var posts = ds_post.ToList();
            foreach (var post in posts)
            {
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
                               ten = user.ho_ten
                               
                           };
                post.like = db.like.Where(like => like.id_post == post.id_post);
                post.img = db.anh.Where(img => img.id_post == post.id_post);

            }
            ViewBag.nhom = db.group.Where(gr => gr.ma_nhom == ma_nhom).FirstOrDefault();
            return View(posts);
        }

        [HttpPost]
        public ActionResult AddPost(HttpPostedFileBase[] uploadFile, string noi_dung_post, int get_id_nhom)
        {
            string username = Session["username"].ToString();
            tnut_socialEntities db = new tnut_socialEntities();
            var new_post = new post
            {
                noi_dung = noi_dung_post,
                uid = username,
                ma_nhom = get_id_nhom,
                ngay_dang = DateTime.Now,
                id_category = 1,
                status = true
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
    }
}