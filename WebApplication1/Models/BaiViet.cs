using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CustomLike
    {
        public string uid;
        public int id_post;
    }
    public class CustomAnh
    {
        public int id_anh;
        public string link;
        public string alt;
        public int id_post;
    }
    public class CustomCmt
    {
        public int id;
        public string uid;
        public int id_post;
        public string nd;
        public string ten;
        public string avatar;
    }
    public class BaiViet
    {
        public string id;
        public int id_post;
        public string nd;
        public string nguoi_dang;
        public string ten_nhom;
        public DateTime time;
        public string avatar;
        public int ma_nhom;
        public bool user_like;
        public List<CustomAnh> img;
        public List<CustomLike> like;
        public List<CustomCmt> cmt;
        public string timeSpan;


    }
}