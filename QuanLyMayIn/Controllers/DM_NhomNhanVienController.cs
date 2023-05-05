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
    public class DM_NhomNhanVienController : Controller
    {

        private Print_LimitEntities db = new Print_LimitEntities();
        private UserInfo user = new UserInfo();

        // GET: DM_NhomNhanVien
        public ActionResult Index()
        {
            if (user.CheckQuyen("DM_NhomNhanVien", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "ID_NhomNhanVien,TenNhomNhanVien,KeyNhomNhanVien,SoLuongBanInTrongThang,GhuChu")] DM_NhomNhanVien dM_NhomNhanVien)
        {
            if (ModelState.IsValid)
            {
                db.DM_NhomNhanVien.Add(dM_NhomNhanVien);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID_NhomNhanVien,TenNhomNhanVien,KeyNhomNhanVien,SoLuongBanInTrongThang,GhuChu")] DM_NhomNhanVien dM_NhomNhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dM_NhomNhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_NhomNhanVien dM_NhomNhanVien = db.DM_NhomNhanVien.Find(id);
            db.DM_NhomNhanVien.Remove(dM_NhomNhanVien);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetSuaNhomNhanVien(int id)
        {
            var data = db.DM_NhomNhanVien.Find(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNhomNhanVien()
        {
            var data = db.DM_NhomNhanVien.Select(_ => new { ID_NhomNhanVien = _.ID_NhomNhanVien, KeyNhomNhanVien = _.KeyNhomNhanVien, TenNhomNhanVien = _.TenNhomNhanVien, GhuChu = _.GhuChu }).ToList();
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
                var nhomtaikhoan = (from nnv in db.DM_NhomNhanVien
                                    select new { ID_NhomNhanVien = nnv.ID_NhomNhanVien, KeyNhomNhanVien = nnv.KeyNhomNhanVien, nnv.SoLuongBanInTrongThang, TenNhomNhanVien = nnv.TenNhomNhanVien, GhuChu = nnv.GhuChu }).ToList();

                recordsTotal = nhomtaikhoan.Count();
                var data1 = nhomtaikhoan.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cang = (from nnv in db.DM_NhomNhanVien
                            where nnv.TenNhomNhanVien.ToUpper().Contains(search.ToUpper()) || nnv.KeyNhomNhanVien.ToUpper().Contains(search.ToUpper())
                            select new { ID_NhomNhanVien = nnv.ID_NhomNhanVien, KeyNhomNhanVien = nnv.KeyNhomNhanVien, nnv.SoLuongBanInTrongThang, TenNhomNhanVien = nnv.TenNhomNhanVien, GhuChu = nnv.GhuChu }).ToList();
                recordsTotal = cang.Count();
                var data1 = cang.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
