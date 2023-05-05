using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QuanLyMayIn.Models;

namespace QuanLyMayIn.Controllers
{
    [Authorize(Roles = "ADMIN, NHAN_VIEN")]
    public class DM_NhanVienController : Controller
    {
        private Print_LimitEntities db = new Print_LimitEntities();
        private UserInfo user = new UserInfo();

        // GET: DM_NhanVien
        public ActionResult Index()
        {
            if (user.CheckQuyen("DM_NhanVien", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            
                return View();
        }

        public ActionResult SoLuongBanIn()
        {
            if (user.CheckQuyen("DM_ChiTietNhanVienMayIn", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            return View();
        }

       
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID_NhanVien,TenNhanVien,DiaChi,SoDienThoai,TenTaiKhoan,MatKhau,Bios_MayTinh,KeyNhomTaiKhoan,KeyNhomNhanVien,SoLuongBanInTrongThang")] DM_NhanVien dM_NhanVien)
        {
            if (ModelState.IsValid)
            {
                dM_NhanVien.SoLuongBanInTrongThang = 0;
                dM_NhanVien.InVoHan = true;
                db.DM_NhanVien.Add(dM_NhanVien);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID_NhanVien,TenNhanVien,DiaChi,SoDienThoai,TenTaiKhoan,MatKhau,Bios_MayTinh,KeyNhomTaiKhoan,KeyNhomNhanVien,SoLuongBanInTrongThang")] DM_NhanVien dM_NhanVien)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(dM_NhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit_NhanVienBanIn(int ID_NhanVien, string KeyNhomNhanVien, int SoLuongBanInTrongThang, string InVoHan)
        {
            var invohan = true;

            if(InVoHan == null)
            {
                invohan = false;
            }
            
            if (KeyNhomNhanVien == "DEFALT")
            {
                var nhanvien = db.DM_NhanVien.Find(ID_NhanVien);
                nhanvien.KeyNhomNhanVien = KeyNhomNhanVien;
                nhanvien.SoLuongBanInTrongThang = SoLuongBanInTrongThang;
                nhanvien.InVoHan = invohan;
                db.Entry(nhanvien).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var nhanvien = db.DM_NhanVien.Find(ID_NhanVien);
                nhanvien.KeyNhomNhanVien = KeyNhomNhanVien;
                nhanvien.InVoHan = invohan;
                db.Entry(nhanvien).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }



        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_NhanVien dM_NhanVien = db.DM_NhanVien.Find(id);
            db.DM_NhanVien.Remove(dM_NhanVien);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetSuaNhanVien(int id)
        {
            var data = (from nv in db.DM_NhanVien
                        join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                        where nv.ID_NhanVien == id
                        select new
                        {
                            ID_NhanVien = nv.ID_NhanVien,
                            TenNhanVien = nv.TenNhanVien,
                            SoDienThoai = nv.SoDienThoai,
                            DiaChi = nv.DiaChi,
                            Bios_MayTinh = nv.Bios_MayTinh,
                            TenTaiKhoan = nv.TenTaiKhoan,
                            nnv.KeyNhomNhanVien,
                            nnv.ID_NhomNhanVien,
                            nnv.TenNhomNhanVien,
                            SoLuongBanInTrongThang = nv.SoLuongBanInTrongThang,
                            InVoHan = nv.InVoHan
                        }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNhanVien()
        {
            var data = (from nv in db.DM_NhanVien
                        join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                        select new { ID_NhanVien = nv.ID_NhanVien, TenNhanVien = nv.TenNhanVien, 
                            SoDienThoai = nv.SoDienThoai, DiaChi = nv.DiaChi, Bios_MayTinh = nv.Bios_MayTinh, 
                            TenTaiKhoan = nv.TenTaiKhoan, 
                            MatKhau = nv.MatKhau,
                            nnv.KeyNhomNhanVien,
                            nnv.ID_NhomNhanVien,
                            nnv.TenNhomNhanVien,
                            nv.InVoHan
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSoLuongBanInNhanVien(int ID_NhanVien, string KeyNhomNhanVien)
        {
            if(KeyNhomNhanVien == "DEFALT")
            {
                int SoLuongBanInTrongThang = (int)db.DM_NhanVien.Find(ID_NhanVien).SoLuongBanInTrongThang;
                return Json(SoLuongBanInTrongThang, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int SoLuongBanInTrongThang = (int)(from nnv in db.DM_NhomNhanVien 
                                              where nnv.KeyNhomNhanVien == KeyNhomNhanVien
                                              select nnv.SoLuongBanInTrongThang).FirstOrDefault();
                return Json(SoLuongBanInTrongThang, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBanInNhanVien(int ID_NhanVien)
        {
            var data = (from nv in db.DM_NhanVien
                        join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                        where nv.ID_NhanVien == ID_NhanVien
                        select new { ID_NhanVien = nv.ID_NhanVien, 
                            TenNhanVien = nv.TenNhanVien, 
                            nnv.KeyNhomNhanVien,
                            nnv.ID_NhomNhanVien,
                            nnv.TenNhomNhanVien,
                            NNVSoLuongBanInTrongThang = nnv.SoLuongBanInTrongThang,
                            NVSoLuongBanInTrongThang = nv.SoLuongBanInTrongThang,
                        }).FirstOrDefault();
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
                            join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                            select new
                            {
                                ID_NhanVien = nv.ID_NhanVien,
                                TenNhanVien = nv.TenNhanVien,
                                SoDienThoai = nv.SoDienThoai,
                                DiaChi = nv.DiaChi,
                                Bios_MayTinh = nv.Bios_MayTinh,
                                TenTaiKhoan = nv.TenTaiKhoan,
                                nnv.KeyNhomNhanVien,
                                nnv.ID_NhomNhanVien,
                                nnv.TenNhomNhanVien,
                            }).ToList();

                recordsTotal = data.Count();
                var data1 = data.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
                var data = (from nv in db.DM_NhanVien
                            join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                            where nv.TenNhanVien.ToUpper().Contains(search.ToUpper()) || nv.SoDienThoai.ToUpper().Contains(search.ToUpper()) 
                            select new
                            {
                                ID_NhanVien = nv.ID_NhanVien,
                                TenNhanVien = nv.TenNhanVien,
                                SoDienThoai = nv.SoDienThoai,
                                DiaChi = nv.DiaChi,
                                Bios_MayTinh = nv.Bios_MayTinh,
                                TenTaiKhoan = nv.TenTaiKhoan,
                                nnv.KeyNhomNhanVien,
                                nnv.ID_NhomNhanVien,
                                nnv.TenNhomNhanVien,
                            }).ToList();
                recordsTotal = data.Count();
                var data1 = data.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LoadDataBanInNhanVien()
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
                            join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                            select new
                            {
                                ID_NhanVien = nv.ID_NhanVien,
                                TenNhanVien = nv.TenNhanVien,
                                nv.Bios_MayTinh,
                                nnv.KeyNhomNhanVien,
                                nnv.ID_NhomNhanVien,
                                nnv.TenNhomNhanVien,
                                NNVSoLuongBanInTrongThang = nnv.SoLuongBanInTrongThang,
                                NVSoLuongBanInTrongThang = nv.SoLuongBanInTrongThang,
                                InVoHan = nv.InVoHan
                            }).ToList();

                recordsTotal = data.Count();
                var data1 = data.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
                var data = (from nv in db.DM_NhanVien
                            join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien

                            select new
                            {
                                ID_NhanVien = nv.ID_NhanVien,
                                TenNhanVien = nv.TenNhanVien,
                                nnv.KeyNhomNhanVien,
                                nv.Bios_MayTinh,
                                nnv.ID_NhomNhanVien,
                                nnv.TenNhomNhanVien,
                                NNVSoLuongBanInTrongThang = nnv.SoLuongBanInTrongThang,
                                NVSoLuongBanInTrongThang = nv.SoLuongBanInTrongThang,
                                InVoHan = nv.InVoHan
                            }).ToList();
                recordsTotal = data.Count();
                var data1 = data.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetNhanVienByIdCookei()
        {
            int userID = user.GetUser();
            var data = db.DM_NhanVien.Where(_ => _.ID_NhanVien == userID).Select(_ => new { Email = _.SoDienThoai, Ma_CanBo = _.ID_NhanVien }).FirstOrDefault();

            return Json( data , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmailTonTai_DoiEmail(string SoDienThoai, int Ma_CanBo)
        {
            var canbo = db.DM_NhanVien.Where(_ => _.SoDienThoai == SoDienThoai && _.ID_NhanVien != Ma_CanBo).Count();

            if (canbo > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EmailTonTai(string SoDienThoai)
        {
            var canbo = db.DM_NhanVien.Where(_ => _.SoDienThoai == SoDienThoai).Count();

            if (canbo > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DoiEmail(int idCanBo, string email1, string email2, string MatKhau1)
        {
            var canbo = db.DM_NhanVien.Find(idCanBo);

            if (canbo.MatKhau != MatKhau1)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                canbo.SoDienThoai = email2;
                db.Entry(canbo).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult CheckTaiKhoan(string TenTaiKhoan)
        {

            var flag = db.DM_NhanVien.Count(x => x.TenTaiKhoan == TenTaiKhoan);

            if (flag > 0)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }



        public JsonResult CheckTaiKhoan1(string TenTaiKhoan, int Ma_CanBo)
        {
            var flag = (from cb in db.DM_NhanVien
                        where cb.TenTaiKhoan == TenTaiKhoan && cb.ID_NhanVien != Ma_CanBo
                        select cb.TenTaiKhoan).Count();

            if (flag > 0)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckBIOS(string Bios_MayTinh)
        {
            var flag = db.DM_NhanVien.Count(x => x.Bios_MayTinh == Bios_MayTinh);

            if (flag > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckEditBIOS(string Bios_MayTinh, int Ma_CanBo)
        {
            var flag = (from cb in db.DM_NhanVien
                        where cb.Bios_MayTinh == Bios_MayTinh && cb.ID_NhanVien != Ma_CanBo
                        select cb.TenTaiKhoan).Count();

            if (flag > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckMatKhau(string MatKhau1)
        {
            var idCanBo = 0;
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                if (!string.IsNullOrEmpty(authCookie.Value))
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    string str = authTicket.UserData;
                    string[] subs = str.Split(',');
                    idCanBo = Int32.Parse(subs[0]);
                }
            }

            var taikhoan = db.DM_NhanVien.Where(_ => _.ID_NhanVien == idCanBo).FirstOrDefault();

            if (taikhoan.MatKhau == MatKhau1)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult DoiMatKhau(string MatKhau1, string MatKhauMoi, string ReMatKhauMoi)
        {
            var idCanBo = 0;
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                if (!string.IsNullOrEmpty(authCookie.Value))
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    string str = authTicket.UserData;
                    string[] subs = str.Split(',');
                    idCanBo = Int32.Parse(subs[0]);
                }
            }

            var taikhoan = db.DM_NhanVien.Where(_ => _.ID_NhanVien == idCanBo).FirstOrDefault();

            if (taikhoan.MatKhau == MatKhau1)
            {
                if (MatKhauMoi == ReMatKhauMoi)
                {
                    taikhoan.MatKhau = MatKhauMoi;
                    db.Entry(taikhoan).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
