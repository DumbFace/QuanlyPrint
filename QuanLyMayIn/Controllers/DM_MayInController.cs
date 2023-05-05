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
    public class DM_MayInController : Controller
    {
        private Print_LimitEntities db = new Print_LimitEntities();
        private UserInfo user = new UserInfo();
        // GET: DM_MayIn
        public ActionResult Index()
        {
            if (user.CheckQuyen("DM_MayIn", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            return View();
        }

      
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID_MayIn,TenMayIn,MaMayIn,GhiChu")] DM_MayIn dM_MayIn)
        {
           
            if (ModelState.IsValid)
            {
                db.DM_MayIn.Add(dM_MayIn);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID_MayIn,TenMayIn,MaMayIn,GhiChu")] DM_MayIn dM_MayIn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dM_MayIn).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_MayIn dM_MayIn = db.DM_MayIn.Find(id);
            db.DM_MayIn.Remove(dM_MayIn);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetSuaMayIn(int id)
        {
            var data = db.DM_MayIn.Find(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMayIn()
        {
            var data = db.DM_MayIn.Select(_ => new { ID_MayIn = _.ID_MayIn, TenMayIn = _.TenMayIn, MaMayIn = _.MaMayIn, GhiChu = _.GhiChu }).ToList();
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
                var nhomtaikhoan = (from mi in db.DM_MayIn
                                    select new { ID_MayIn = mi.ID_MayIn, TenMayIn = mi.TenMayIn, MaMayIn = mi.MaMayIn, GhiChu = mi.GhiChu }).ToList();

                recordsTotal = nhomtaikhoan.Count();
                var data1 = nhomtaikhoan.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cang = (from mi in db.DM_MayIn
                            where mi.TenMayIn.ToUpper().Contains(search.ToUpper()) || mi.MaMayIn.ToUpper().Contains(search.ToUpper())
                            select new { ID_MayIn = mi.ID_MayIn, TenMayIn = mi.TenMayIn, MaMayIn = mi.MaMayIn, GhiChu = mi.GhiChu }).ToList();
                recordsTotal = cang.Count();
                var data1 = cang.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
