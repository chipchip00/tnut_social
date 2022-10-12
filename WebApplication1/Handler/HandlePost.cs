using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Handler
{
    public class HandlePost
    {
        tnut_socialEntities db = new tnut_socialEntities();
        public string username;
        public int? maNhom;
        public int pageNumber = 0;
        public bool status = true;

        const int POSTS_PER_PAGE = 10; 


        public HandlePost(string username, int ?maNhom, int pageNumber)
        {
            this.username = username;
            this.maNhom = maNhom;
            this.pageNumber = pageNumber;
        }
        public List<BaiViet> getPosts()
        {
            IQueryable<BaiViet> ds_post;
            if(maNhom != null)
            {
                ds_post  = from dt_post in db.post
                            join dt_user in db.user on dt_post.uid equals dt_user.uid
                            join dt_group in db.@group on dt_post.ma_nhom equals dt_group.ma_nhom
                            where (dt_group.ma_nhom == maNhom && dt_post.status == status)
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
            }
            else
            {
                var data_nhom = (from dt_nhom in db.user_group
                              where dt_nhom.uid == username
                              select dt_nhom.ma_nhom).ToList();
                ds_post = from dt_post in db.post
                          join dt_user in db.user on dt_post.uid equals dt_user.uid
                          join dt_group in db.@group on dt_post.ma_nhom equals dt_group.ma_nhom
                          where (data_nhom.Contains((int)dt_post.ma_nhom) && dt_post.status == true)
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
                              ma_nhom = (int)dt_post.ma_nhom,
                          };
            }

            var posts = ds_post.Skip(pageNumber * POSTS_PER_PAGE).Take(POSTS_PER_PAGE).ToList();

            foreach (var post in posts)
            {
                var timeSpan = DateTime.Now - post.time;
                post.timeSpan = (
                    (timeSpan).Days > 0 ?
                            (timeSpan).Days.ToString() + " ngày trước"
                            : (
                                (timeSpan).Hours.ToString()
                                + " giờ "
                                + (timeSpan).Minutes.ToString()
                                + " phút trước."
                            )
                        );
                post.cmt = (from cmt in db.comment
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
                                link = imgs.link,
                                alt = imgs.mo_ta

                            }).ToList();
                var dt_user_like = db.like.Where(dt => dt.id_post == post.id_post && dt.uid == username).FirstOrDefault();
                if (dt_user_like != null) post.user_like = true;
                else post.user_like = false;
            }
            return posts;
        }
    }
}