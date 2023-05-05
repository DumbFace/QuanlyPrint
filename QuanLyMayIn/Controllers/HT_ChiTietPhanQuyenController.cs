using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyMayIn.Models;

namespace QuanLyMayIn.Controllers
{
    [Authorize(Roles = "ADMIN, NHAN_VIEN")]
    public class HT_ChiTietPhanQuyenController : Controller
    {
        private Print_LimitEntities db = new Print_LimitEntities();
        private UserInfo userInfo = new UserInfo();

        // GET: HT_ChiTietPhanQuyen
        public ActionResult Index()
        {
            if (userInfo.CheckQuyen("HT_ChiTietPhanQuyen", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            return View();
        }

        
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID_ChiTietPhanQuyen,KeyNhomTaiKhoan,KeyMenu,KeyChucNang,TrangThai")] HT_ChiTietPhanQuyen hT_ChiTietPhanQuyen)
        {
            if (ModelState.IsValid)
            {
                db.HT_ChiTietPhanQuyen.Add(hT_ChiTietPhanQuyen);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(List<int> ID_ChiTietPhanQuyen, string MaNhomQuyen)
        {
            if (ModelState.IsValid)
            {
                //update MaNhomQuyen thành fasle
                var getCHiTietChucNangByNhomQuyen = db.HT_ChiTietPhanQuyen.Where(_ => _.KeyNhomTaiKhoan == MaNhomQuyen).ToList();
                foreach(var i in getCHiTietChucNangByNhomQuyen)
                {
                    i.TrangThai = false;
                    db.Entry(i).State = EntityState.Modified;
                }
                db.SaveChanges();
                foreach (var i in ID_ChiTietPhanQuyen)
                {
                    var chitietphanquyen = db.HT_ChiTietPhanQuyen.Find(i);
                    chitietphanquyen.TrangThai = true;
                    db.Entry(chitietphanquyen).State = EntityState.Modified;
                }
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            HT_ChiTietPhanQuyen hT_ChiTietPhanQuyen = db.HT_ChiTietPhanQuyen.Find(id);
            db.HT_ChiTietPhanQuyen.Remove(hT_ChiTietPhanQuyen);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetMenuPhanQuyen()
        {
            var data = db.HT_Menu.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChiTietPhanQuyen(string MaNhomQuyen, string Menu)
        {
            var data = (from ctpq in db.HT_ChiTietPhanQuyen
                        join cn in db.HT_ChucNang on ctpq.KeyChucNang equals cn.KeyChucNang
                        join mn in db.HT_Menu on ctpq.KeyMenu equals mn.KeyMenu
                        join ntk in db.DM_NhomTaiKhoan on ctpq.KeyNhomTaiKhoan equals ntk.KeyNhomTaiKhoan
                        where mn.KeyMenu == Menu && ntk.KeyNhomTaiKhoan == MaNhomQuyen
                        select new { mn.ID_Menu, mn.KeyMenu, mn.TenMenu, ntk.KeyNhomTaiKhoan, ntk.TenNhomTaiKhoan, cn.KeyChucNang, cn.TenChucNang, ctpq.TrangThai, ctpq.ID_ChiTietPhanQuyen}).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQuyen(string MenuCode)
        {
            var username = userInfo.GetTaiKhoan();
            var tk = db.DM_NhanVien.FirstOrDefault(_ => _.TenTaiKhoan == username).KeyNhomTaiKhoan;

            var data = db.HT_ChiTietPhanQuyen.Where(_ => _.KeyNhomTaiKhoan == tk && _.TrangThai == true && _.KeyMenu.ToUpper() == MenuCode.ToUpper()).Select(_ => _.KeyChucNang.ToUpper());

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
