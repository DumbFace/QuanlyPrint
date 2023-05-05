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
    public class DM_ChiTietNhanVienMayInController : Controller
    {
        private Print_LimitEntities db = new Print_LimitEntities();
        private UserInfo user = new UserInfo();

        // GET: DM_ChiTietNhanVienMayIn
        public ActionResult Index()
        {

            if (user.CheckQuyen("DM_ChiTietNhanVienMayIn", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            return View();
        }

        
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID_ChiTietNhanVienMayIn,ID_NhanVien")] DM_ChiTietNhanVienMayIn dM_ChiTietNhanVienMayIn, List<int> ID_MayIn)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in ID_MayIn)
                {
                    dM_ChiTietNhanVienMayIn.ID_MayIn = item;
                    db.DM_ChiTietNhanVienMayIn.Add(dM_ChiTietNhanVienMayIn);
                    db.SaveChanges();
                }
               
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID_NhanVien")] DM_ChiTietNhanVienMayIn dM_ChiTietNhanVienMayIn, List<int> ID_MayIn)
        {

            var chitietnhanvienmahin = db.DM_ChiTietNhanVienMayIn.Where(_ => _.ID_NhanVien == dM_ChiTietNhanVienMayIn.ID_NhanVien).ToList();
            if (chitietnhanvienmahin.Count() != 0)
            {
                foreach (var item in chitietnhanvienmahin)
                {
                    db.DM_ChiTietNhanVienMayIn.Remove(item);
                    db.SaveChanges();
                }
            }
            if (ModelState.IsValid)
            {
                foreach (var item in ID_MayIn)
                {
                    dM_ChiTietNhanVienMayIn.ID_MayIn = item;
                    db.DM_ChiTietNhanVienMayIn.Add(dM_ChiTietNhanVienMayIn);
                    db.SaveChanges();
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            var chitietnhanvienmahin = db.DM_ChiTietNhanVienMayIn.Where(_ => _.ID_NhanVien == id).ToList();
            if (chitietnhanvienmahin.Count() != 0)
            {
                foreach (var item in chitietnhanvienmahin)
                {
                    db.DM_ChiTietNhanVienMayIn.Remove(item);
                    db.SaveChanges();
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetSuaChiTietNhanVienMayIn(int ID_NhanVien)
        {
            var data = (from ctnvmi in db.DM_ChiTietNhanVienMayIn
                        where ctnvmi.ID_NhanVien == ID_NhanVien
                        select ctnvmi).ToList();
    
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetSuaChiTietNhanVienMayIn_MayInCuaNhanVien(int ID_NhanVien)

        [HttpGet]
        public ActionResult GetChiTietNhanVienMayIn()
        {
            var data = (from nv in db.DM_NhanVien
                        select new { nv.ID_NhanVien, nv.TenNhanVien, MayIn = (from mi in db.DM_MayIn join ctnvmi in db.DM_ChiTietNhanVienMayIn on mi.ID_MayIn equals ctnvmi.ID_MayIn where ctnvmi.ID_NhanVien == nv.ID_NhanVien select mi).ToList()}).ToList();
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
                var data = (from nv in db.DM_NhanVien
                                    select new { nv.ID_NhanVien, nv.TenNhanVien, nv.Bios_MayTinh, MayIn = (from mi in db.DM_MayIn join ctnvmi in db.DM_ChiTietNhanVienMayIn on mi.ID_MayIn equals ctnvmi.ID_MayIn where ctnvmi.ID_NhanVien == nv.ID_NhanVien select mi).ToList() }).ToList();

                recordsTotal = data.Count();
                var data1 = data.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cang = (from nv in db.DM_NhanVien
                            where nv.TenNhanVien.ToUpper().Contains(search.ToUpper())
                            select new { nv.ID_NhanVien, nv.TenNhanVien, MayIn = (from mi in db.DM_MayIn join ctnvmi in db.DM_ChiTietNhanVienMayIn on mi.ID_MayIn equals ctnvmi.ID_MayIn where ctnvmi.ID_NhanVien == nv.ID_NhanVien select mi).ToList() }).ToList();
                recordsTotal = cang.Count();
                var data1 = cang.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
