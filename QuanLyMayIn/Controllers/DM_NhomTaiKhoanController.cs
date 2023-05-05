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
    [Authorize(Roles = "ADMIN, NhanVien")]
    public class DM_NhomTaiKhoanController : Controller
    {
        private Print_LimitEntities db = new Print_LimitEntities();
        private UserInfo user = new UserInfo();

        
        public ActionResult Index()
        {
            if (user.CheckQuyen("DM_NhomTaiKhoan", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            return View();
        }

        
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID_NhomTaiKhoan,KeyNhomTaiKhoan,TenNhomTaiKhoan,GhiChu")] DM_NhomTaiKhoan dM_NhomTaiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.DM_NhomTaiKhoan.Add(dM_NhomTaiKhoan);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID_NhomTaiKhoan,KeyNhomTaiKhoan,TenNhomTaiKhoan,GhiChu")] DM_NhomTaiKhoan dM_NhomTaiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dM_NhomTaiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_NhomTaiKhoan dM_NhomTaiKhoan = db.DM_NhomTaiKhoan.Find(id);
            db.DM_NhomTaiKhoan.Remove(dM_NhomTaiKhoan);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetSuaNhomTaiKhoan(int id)
        {
            var data = db.DM_NhomTaiKhoan.Find(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNhomTaiKhoan()
        {
            var data = db.DM_NhomTaiKhoan.Select(_ => new { ID_NhomTaiKhoan = _.ID_NhomTaiKhoan, KeyNhomTaiKhoan = _.KeyNhomTaiKhoan, TenNhomTaiKhoan = _.TenNhomTaiKhoan, GhiChu = _.GhiChu }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadData()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var search = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

            if (string.IsNullOrEmpty(search))
            {
                var nhomtaikhoan = (from ntk in db.DM_NhomTaiKhoan
                            select new { ID_NhomTaiKhoan = ntk.ID_NhomTaiKhoan, KeyNhomTaiKhoan = ntk.KeyNhomTaiKhoan, TenNhomTaiKhoan = ntk.TenNhomTaiKhoan, GhiChu = ntk.GhiChu }).ToList();

                recordsTotal = nhomtaikhoan.Count();
                var data1 = nhomtaikhoan.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cang = (from ntk in db.DM_NhomTaiKhoan
                            where ntk.TenNhomTaiKhoan.ToUpper().Contains(search.ToUpper()) || ntk.KeyNhomTaiKhoan.ToUpper().Contains(search.ToUpper())
                            select new { ID_NhomTaiKhoan = ntk.ID_NhomTaiKhoan, KeyNhomTaiKhoan = ntk.KeyNhomTaiKhoan, TenNhomTaiKhoan = ntk.TenNhomTaiKhoan, GhiChu = ntk.GhiChu }).ToList();
                recordsTotal = cang.Count();
                var data1 = cang.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
