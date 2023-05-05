using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QuanLyMayIn.Models;
namespace QuanLyMayIn.Controllers


{
    [Authorize(Roles = "ADMIN, NHAN_VIEN")]
    public class SharedController : Controller
    {


        // GET: Shared
        // GET: Shared
        private Print_LimitEntities db = new Print_LimitEntities();
        private UserInfo user = new UserInfo();
      

      
        [ChildActionOnly]
        public ActionResult RenderUserInfo()
        {
            int userid = user.GetUser();
            //var thongbao = db.HT_ThongBao.Where(_ => _.NguoiNhan == user && _.TrangThai == true).Count();

            //ViewBag.thongbao = thongbao;

            //ViewBag.Id = getuser();

            //ViewBag.TenUser = db.HT_NhanVien.Find(user).TenNhanVien;

            ViewBag.TenNhanVien = db.DM_NhanVien.Find(userid).TenNhanVien;
            return PartialView("_LayoutUserInfo");
        }
        [ChildActionOnly]
        public ActionResult RenderMenu()
        {
            string userRole = user.GetGroupKey();

            ViewBag.ListQuyen = db.HT_ChiTietPhanQuyen.Where(_ => _.KeyNhomTaiKhoan == userRole && _.TrangThai == true).Select(_ => _.KeyMenu + "_" + _.KeyChucNang).ToList();
            return PartialView("TreeviewLeftMenu");
        }

    }
}