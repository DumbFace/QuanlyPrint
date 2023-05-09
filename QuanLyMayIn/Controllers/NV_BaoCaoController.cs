
using QuanLyMayIn.Models;
using ExcelDataReader;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using OpenXmlPowerTools;

namespace QuanLyMayIn.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class NV_BaoCaoController : Controller
    {

        private Print_LimitEntities db = new Print_LimitEntities();
        private UserInfo user = new UserInfo();

        // GET: NV_BaoCao
        public ActionResult Index()
        {
            if (user.CheckQuyen("NV_BaoCao", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            return View();
        }

        public ActionResult MayIn()
        {
            if (user.CheckQuyen("NV_BaoCao", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            return View();
        }

        public ActionResult Nhom()
        {
            if (user.CheckQuyen("NV_BaoCao", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            return View();
        }


        public ActionResult TongHop()
        {
            if (user.CheckQuyen("NV_BaoCao", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            return View();
        }



        public ActionResult XemChiTiet(int ID_NhanVien, DateTime ?Filter)
        {
            if (user.CheckQuyen("NV_BaoCao", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            ViewBag.ID_NhanVien = ID_NhanVien;
            ViewBag.Filter = Filter;
            return View();
        }
        
        public ActionResult XemChiTietMayIn(int ID_MayIn, DateTime ?Filter)
        {
            if (user.CheckQuyen("NV_BaoCao", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            ViewBag.ID_MayIn = ID_MayIn;
            ViewBag.Filter = Filter;
            return View();
        }

        public ActionResult XemChiTietNhom(int ID_NhomNhanVien, DateTime? Filter)
        {
            if (user.CheckQuyen("NV_BaoCao", "XEM"))
            {
                return new HttpStatusCodeResult(404, "Not found");
            }
            ViewBag.ID_NhomNhanVien = ID_NhomNhanVien;
            ViewBag.Filter = Filter;
            return View();
        }


        public JsonResult LoadData()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var search = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var filter = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();


            DateTime filterMonth = new DateTime((filter != "") ?  Int32.Parse(filter.Split('-')[0]) : 2000, (filter != "") ? Int32.Parse(filter.Split('-')[1]) : 1, 1);
            
            var UserID = user.GetUser();
            
            var data = (from bi in db.NV_BanIn
                        join nv in db.DM_NhanVien on bi.ID_NhanVien equals nv.ID_NhanVien
                        join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                        select new { nv.ID_NhanVien, bi.ID_BanIn, bi.JobID, bi.MaTaiLieu, bi.TenTaiLieuDinhKem, bi.ThoiGianPrint, bi.TrangThaiText, bi.TrangThai, bi.TongSoTrangDaIn, bi.TongSoTrang, bi.ThoiGianUpload, nv.KeyNhomNhanVien, nv.Bios_MayTinh, nnv.TenNhomNhanVien, bi.TenMayIn, SoLuongBanInTrongThang = (nv.KeyNhomNhanVien == "DEFALT") ? nv.SoLuongBanInTrongThang : nnv.SoLuongBanInTrongThang, nv.TenNhanVien }).ToList();

            if (filterMonth.Year != 2000)
            {
                data = data.Where(_ => _.ThoiGianPrint != null && _.ThoiGianPrint.Value.Month == filterMonth.Month && _.ThoiGianPrint.Value.Year == filterMonth.Year).ToList();
            }
            var data2 = (from dt in data
                         group dt by  dt.ID_NhanVien into gr
                         select new {gr.First().ID_NhanVien, gr.First().TenNhanVien, gr.First().Bios_MayTinh, gr.First().SoLuongBanInTrongThang, gr.First().TenNhomNhanVien, gr.First().KeyNhomNhanVien, ThoiGianPrint = (gr.First().ThoiGianPrint == null) ? new DateTime(2000, 1,1) : (DateTime)gr.First().ThoiGianPrint, TongSoTrangDaIn = gr.Sum(_ => _.TongSoTrangDaIn)}).ToList();
            
            
            
            var KeyNhomTaiTaiKhoan = user.GetGroupKey();


            if (KeyNhomTaiTaiKhoan != "ADMIN")
            {
                data2 = data2.Where(_ => _.ID_NhanVien == UserID).ToList();
            }

            
            if (!string.IsNullOrEmpty(search))
            {
                data2 = data2.Where(_ => _.TenNhanVien.ToUpper().Contains(search.ToUpper())).ToList();
            }
            

            recordsTotal = data2.Count();
            var data1 = data2.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadDataBaoCaoMayIn()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var search = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var filter = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();


            DateTime filterMonth = new DateTime((filter != "") ?  Int32.Parse(filter.Split('-')[0]) : 2000, (filter != "") ? Int32.Parse(filter.Split('-')[1]) : 1, 1);
            
            var UserID = user.GetUser();
            
            var data = (from bi in db.NV_BanIn
                        join nv in db.DM_NhanVien on bi.ID_NhanVien equals nv.ID_NhanVien
                        join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                        join mi in db.DM_MayIn on bi.TenMayIn equals mi.TenMayIn
                        select new { mi.ID_MayIn, mi.TenMayIn, bi.ID_BanIn, bi.JobID, bi.MaTaiLieu, bi.TenTaiLieuDinhKem, bi.ThoiGianPrint, bi.TrangThaiText, bi.TrangThai, bi.TongSoTrangDaIn, bi.TongSoTrang, bi.ThoiGianUpload, nv.KeyNhomNhanVien, nv.Bios_MayTinh, nnv.TenNhomNhanVien, SoLuongBanInTrongThang = (nv.KeyNhomNhanVien == "DEFALT") ? nv.SoLuongBanInTrongThang : nnv.SoLuongBanInTrongThang, nv.TenNhanVien }).ToList();

            if (filterMonth.Year != 2000)
            {
                data = data.Where(_ => _.ThoiGianPrint != null && _.ThoiGianPrint.Value.Month == filterMonth.Month && _.ThoiGianPrint.Value.Year == filterMonth.Year).ToList();
            }
            var data2 = (from dt in data
                         group dt by  dt.ID_MayIn into gr
                         select new {gr.First().ID_MayIn, gr.First().TenMayIn, ThoiGianPrint = (gr.First().ThoiGianPrint == null) ? new DateTime(2000, 1,1) : (DateTime)gr.First().ThoiGianPrint, TongSoTrangDaIn = gr.Sum(_ => _.TongSoTrangDaIn)}).ToList();
            
                        var KeyNhomTaiTaiKhoan = user.GetGroupKey();

            

            if (!string.IsNullOrEmpty(search))
            {
                data2 = data2.Where(_ => _.TenMayIn.ToUpper().Contains(search.ToUpper())).ToList();
            }
            

            recordsTotal = data2.Count();
            var data1 = data2.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LoadDataBaoCaoNhom()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var search = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var filter = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();


            DateTime filterMonth = new DateTime((filter != "") ? Int32.Parse(filter.Split('-')[0]) : 2000, (filter != "") ? Int32.Parse(filter.Split('-')[1]) : 1, 1);

            var UserID = user.GetUser();

            var data = (from bi in db.NV_BanIn
                        join nv in db.DM_NhanVien on bi.ID_NhanVien equals nv.ID_NhanVien
                        join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                        select new {  nnv.ID_NhomNhanVien, nnv.KeyNhomNhanVien, nnv.TenNhomNhanVien, bi.TenMayIn, bi.ID_BanIn, bi.JobID, bi.MaTaiLieu, bi.TenTaiLieuDinhKem, bi.ThoiGianPrint, bi.TrangThaiText, bi.TrangThai, bi.TongSoTrangDaIn, bi.TongSoTrang, bi.ThoiGianUpload,  nv.Bios_MayTinh,  SoLuongBanInTrongThang = (nv.KeyNhomNhanVien == "DEFALT") ? nv.SoLuongBanInTrongThang : nnv.SoLuongBanInTrongThang, nv.TenNhanVien }).ToList();

            if (filterMonth.Year != 2000)
            {
                data = data.Where(_ => _.ThoiGianPrint != null && _.ThoiGianPrint.Value.Month == filterMonth.Month && _.ThoiGianPrint.Value.Year == filterMonth.Year).ToList();
            }
            var data2 = (from dt in data
                         group dt by dt.KeyNhomNhanVien into gr
                         select new { gr.First().ID_NhomNhanVien, gr.First().TenNhomNhanVien, ThoiGianPrint = (gr.First().ThoiGianPrint == null) ? new DateTime(2000, 1, 1) : (DateTime)gr.First().ThoiGianPrint, TongSoTrangDaIn = gr.Sum(_ => _.TongSoTrangDaIn) }).ToList();

            var KeyNhomTaiTaiKhoan = user.GetGroupKey();



            if (!string.IsNullOrEmpty(search))
            {
                data2 = data2.Where(_ => _.TenNhomNhanVien.ToUpper().Contains(search.ToUpper())).ToList();
            }


            recordsTotal = data2.Count();
            var data1 = data2.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult LoadDataChiTiet(int ID_NhanVien, DateTime? month)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var search = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var filter = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var UserID = user.GetUser();

            var data = (from bi in db.NV_BanIn
                        join nv in db.DM_NhanVien on bi.ID_NhanVien equals nv.ID_NhanVien
                        join nnv in db.DM_NhomTaiKhoan on nv.KeyNhomTaiKhoan equals nnv.KeyNhomTaiKhoan
                        where nv.ID_NhanVien == ID_NhanVien 
                        select new { bi.ID_NhanVien, bi.ID_BanIn, bi.JobID, bi.MaTaiLieu, bi.TenTaiLieuDinhKem, bi.ThoiGianPrint, bi.TrangThaiText, bi.TrangThai, bi.TongSoTrangDaIn, bi.TongSoTrang, bi.ThoiGianUpload, nv.TenNhanVien, nv.KeyNhomNhanVien, nv.Bios_MayTinh, nnv.TenNhomTaiKhoan, bi.TenMayIn , pageSize}).ToList();
            
            if(month != null)
            {
                data = data.Where(_ => _.ThoiGianPrint.Value.Month == month.Value.Month && _.ThoiGianPrint.Value.Year == month.Value.Year).ToList();
            }
            var KeyNhomTaiTaiKhoan = user.GetGroupKey();


            if (KeyNhomTaiTaiKhoan != "ADMIN")
            {
                data = data.Where(_ => _.ID_NhanVien == UserID).ToList();
            }

            if (string.IsNullOrEmpty(search))
            {
                data = data.Where(_ => _.TenTaiLieuDinhKem.ToUpper().Contains(search.ToUpper())).ToList();
            }

            recordsTotal = data.Count();
            var data1 = data.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
        }
         public ActionResult LoadDataChiTietMayIn(int ID_MayIn, DateTime? month)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var search = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var UserID = user.GetUser();


            var data = (from bi in db.NV_BanIn
                        join nv in db.DM_NhanVien on bi.ID_NhanVien equals nv.ID_NhanVien
                        join nnv in db.DM_NhomTaiKhoan on nv.KeyNhomTaiKhoan equals nnv.KeyNhomTaiKhoan
                        join mi in db.DM_MayIn on bi.TenMayIn equals mi.TenMayIn
                        where  mi.ID_MayIn == ID_MayIn 
                        select new { bi.ID_NhanVien, bi.ID_BanIn, bi.JobID, bi.MaTaiLieu, bi.TenTaiLieuDinhKem, bi.ThoiGianPrint, bi.TrangThaiText, bi.TrangThai, bi.TongSoTrangDaIn, bi.TongSoTrang, bi.ThoiGianUpload, nv.TenNhanVien, nv.KeyNhomNhanVien, nv.Bios_MayTinh, nnv.TenNhomTaiKhoan, bi.TenMayIn }).ToList();
            
            if(month != null)
            {
                data = data.Where(_ => _.ThoiGianPrint.Value.Month == month.Value.Month && _.ThoiGianPrint.Value.Year == month.Value.Year).ToList();
            }
            var KeyNhomTaiTaiKhoan = user.GetGroupKey();


            if (KeyNhomTaiTaiKhoan != "ADMIN")
            {
                data = data.Where(_ => _.ID_NhanVien == UserID).ToList();
            }

            if (string.IsNullOrEmpty(search))
            {
                data = data.Where(_ => _.TenTaiLieuDinhKem.ToUpper().Contains(search.ToUpper())).ToList();
            }

            recordsTotal = data.Count();
            var data1 = data.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDataChiTietNhom(int ID_NhomNhanVien, DateTime? month)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var search = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var filter = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var monthFilter = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();

            var UserID = user.GetUser();


            var data = (from bi in db.NV_BanIn
                        join nv in db.DM_NhanVien on bi.ID_NhanVien equals nv.ID_NhanVien
                        join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                        where nnv.ID_NhomNhanVien == ID_NhomNhanVien
                        orderby bi.ThoiGianPrint descending
                        select new { bi.ID_NhanVien, bi.ID_BanIn, bi.JobID, bi.MaTaiLieu, bi.TenTaiLieuDinhKem, bi.ThoiGianPrint, bi.TrangThaiText, bi.TrangThai, bi.TongSoTrangDaIn, bi.TongSoTrang, bi.ThoiGianUpload, nv.TenNhanVien, nv.KeyNhomNhanVien, nv.Bios_MayTinh, nnv.TenNhomNhanVien, bi.TenMayIn, bi.PaperSize }).ToList();

            if (month != null)
            {
                data = data.Where(_ => _.ThoiGianPrint.Value.Month == month.Value.Month && _.ThoiGianPrint.Value.Year == month.Value.Year).ToList();
            }
            var KeyNhomTaiTaiKhoan = user.GetGroupKey();

            if (KeyNhomTaiTaiKhoan != "ADMIN")
            {
                data = data.Where(_ => _.ID_NhanVien == UserID).ToList();
            }

            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(_ => _.TenTaiLieuDinhKem.ToUpper().Contains(search.ToUpper())).ToList();
            }
            if (!string.IsNullOrEmpty(filter))
            {
                if(filter != "all")
                {
                    if (filter == "null" || filter == null)
                    {
                        data = data.Where(_ => _.PaperSize == "" || _.PaperSize == null).ToList();
                    }
                    else
                    {
                        data = data.Where(_ => _.PaperSize == filter).ToList();
                    }
                    
                }
            }
            recordsTotal = data.Count();
            var data1 = data.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TotalChiTietNhom(int ID_NhomNhanVien, DateTime? month, string PaperSize)
        {


            var UserID = user.GetUser();
            var data = (from bi in db.NV_BanIn
                        join nv in db.DM_NhanVien on bi.ID_NhanVien equals nv.ID_NhanVien
                        join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                        where nnv.ID_NhomNhanVien == ID_NhomNhanVien
                        select new { bi.ID_NhanVien, bi.ID_BanIn, bi.JobID, bi.MaTaiLieu, bi.TenTaiLieuDinhKem, bi.ThoiGianPrint, bi.TrangThaiText, bi.TrangThai, bi.TongSoTrangDaIn, bi.TongSoTrang, bi.ThoiGianUpload, nv.TenNhanVien, nv.KeyNhomNhanVien, nv.Bios_MayTinh, nnv.TenNhomNhanVien, bi.TenMayIn, bi.PaperSize }).ToList();

            if (PaperSize != "all")
            {
                if (PaperSize == "null" || PaperSize == null)
                {
                    data = data.Where(_ => _.PaperSize == "" || _.PaperSize == null).ToList();
                }
                else
                {
                    data = data.Where(_ => _.PaperSize == PaperSize).ToList();
                }

            }
            if (month != null)
            {
                data = data.Where(_ => _.ThoiGianPrint.Value.Month == month.Value.Month && _.ThoiGianPrint.Value.Year == month.Value.Year).ToList();
            }
            var KeyNhomTaiTaiKhoan = user.GetGroupKey();

            if (KeyNhomTaiTaiKhoan != "ADMIN")
            {
                data = data.Where(_ => _.ID_NhanVien == UserID).ToList();
            }

          
           
            return Json(data.Sum(_=> _.TongSoTrangDaIn), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadCountSumYear()
        {
            var data = 0;
            TimeSpan ts = new TimeSpan(365, 0, 0, 0);
            DateTime yearAgo = DateTime.Now;

            var changesPerYearAndMonth =
            (from year in Enumerable.Range(yearAgo.Year, 1)
            from month in Enumerable.Range(1, 12)
            let key = new { Year = year, Month = month }
            join bi in db.NV_BanIn  on key
                  equals new
                  {
                      bi.ThoiGianPrint.Value.Year,
                      bi.ThoiGianPrint.Value.Month
                  } into g
            select new { GroupCriteria = key, Count = g.Sum(_ => _.TongSoTrangDaIn) }).ToList();
            return Json(changesPerYearAndMonth, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Filter()
        {
            var data = (from bi in db.NV_BanIn
                        group bi by bi.PaperSize into g
                        select new { PaperSize = g.Key }).ToList();
           
                return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InBaoCao(int ID_NhomNhanVien, DateTime? month)
        {
            
            var data = (from bi in db.NV_BanIn
                        join nv in db.DM_NhanVien on bi.ID_NhanVien equals nv.ID_NhanVien
                        join nnv in db.DM_NhomNhanVien on nv.KeyNhomNhanVien equals nnv.KeyNhomNhanVien
                        where nnv.ID_NhomNhanVien == ID_NhomNhanVien 
                        select new { bi.ID_NhanVien, nv.TenNhanVien, bi.ID_BanIn, bi.JobID, bi.MaTaiLieu, bi.TenTaiLieuDinhKem, bi.ThoiGianPrint, bi.TrangThaiText, bi.TrangThai, bi.TongSoTrangDaIn, bi.TongSoTrang, bi.ThoiGianUpload, nv.KeyNhomNhanVien, nv.Bios_MayTinh, nnv.TenNhomNhanVien, bi.TenMayIn, bi.PaperSize }).ToList();
            if (month != null)
            {
                data = data.Where(_ => _.ThoiGianPrint.Value.Month == month.Value.Month && _.ThoiGianPrint.Value.Year == month.Value.Year).ToList();
            }
            var sum = data.Sum(_ => _.TongSoTrangDaIn);
            var fileName = $@"BaoCaoIn.xlsx";
            var file = new FileInfo(fileName);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            var package = new ExcelPackage(file);
            if (data.Count == 0)
            {
                return Json(new { status = "error", message = "Cán bộ chưa được điểm danh trong thời gian này" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
         
                var worksheet1 = package.Workbook.Worksheets.Add("BaoCao");

                worksheet1.Cells[1, 1, 1, 9].Merge = true;

                //worksheet1.Cells[5, 1, 6, 1].Merge = true;
                //worksheet1.Cells[5, 2, 6, 2].Merge = true;
                //worksheet1.Cells[5, 3, 6, 3].Merge = true;
                //worksheet1.Cells[5, 4, 5, 5].Merge = true;
                //worksheet1.Cells[5, 6, 6, 6].Merge = true;
                //worksheet1.Cells[5, 7, 6, 7].Merge = true;
                //worksheet1.Cells[5, 8, 6, 8].Merge = true;
                //worksheet1.Cells[5, 9, 6, 9].Merge = true;
                //worksheet1.Cells[5, 10, 6, 10].Merge = true;


                worksheet1.Column(1).Width = GetTrueColumnWidth(35.67);
                worksheet1.Column(2).Width = GetTrueColumnWidth(12.11);
                worksheet1.Column(3).Width = GetTrueColumnWidth(10.67);
                worksheet1.Column(4).Width = GetTrueColumnWidth(10.11);
                worksheet1.Column(5).Width = GetTrueColumnWidth(25.67);
                worksheet1.Column(6).Width = GetTrueColumnWidth(10.44);
                worksheet1.Column(7).Width = GetTrueColumnWidth(45.44);
                worksheet1.Column(8).Width = GetTrueColumnWidth(25.44);
                worksheet1.Column(9).Width = GetTrueColumnWidth(20.44);

                worksheet1.Cells["A1:I1"].Value = $@"Báo Cáo In Ấn";
                worksheet1.Cells["A1:I1"].Style.Font.Bold = true;
                worksheet1.Cells["A1:I1"].Style.Font.Size = 20;
                worksheet1.Cells["A1:I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                worksheet1.Cells["A3"].Value = $@"Họ Tên";
                worksheet1.Cells["A3"].Style.Font.Bold = true;
                worksheet1.Cells["A3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet1.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet1.Cells["B3"].Value = "IP Máy Tính";
                worksheet1.Cells["B3"].Style.Font.Bold = true;
                worksheet1.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet1.Cells["B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet1.Cells["C3"].Value = "Ngày In";
                worksheet1.Cells["C3"].Style.Font.Bold = true;
                worksheet1.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet1.Cells["C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet1.Cells["D3"].Value = "Giờ In";
                worksheet1.Cells["D3"].Style.Font.Bold = true;
                worksheet1.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet1.Cells["D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet1.Cells["E3"].Value = "Tên Tài Liệu";
                worksheet1.Cells["E3"].Style.Font.Bold = true;
                worksheet1.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet1.Cells["E3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet1.Cells["F3"].Value = "Trang đã in";
                worksheet1.Cells["F3"].Style.Font.Bold = true;
                worksheet1.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet1.Cells["F3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet1.Cells["G3"].Value = "Khổ Giấy";
                worksheet1.Cells["G3"].Style.Font.Bold = true;
                worksheet1.Cells["G3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet1.Cells["G3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet1.Cells["H3"].Value = "Tên Máy In";
                worksheet1.Cells["H3"].Style.Font.Bold = true;
                worksheet1.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet1.Cells["H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet1.Cells["I3"].Value = "Trạng Thái";
                worksheet1.Cells["I3"].Style.Font.Bold = true;
                worksheet1.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet1.Cells["I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int row = 4;
                foreach (var item in data)
                {

                    worksheet1.Cells[string.Format("A{0}", row)].Value = item.TenNhanVien;
                    worksheet1.Cells[$"A{row}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet1.Cells[$"A{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet1.Cells[string.Format("B{0}", row)].Value = item.Bios_MayTinh;
                    worksheet1.Cells[$"B{row}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet1.Cells[$"B{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet1.Cells[string.Format("C{0}", row)].Value = $"{item.ThoiGianPrint.Value.ToString("dd/MM/yyyy")}";
                    worksheet1.Cells[$"C{row}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet1.Cells[$"C{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet1.Cells[string.Format("D{0}", row)].Value = $"{item.ThoiGianPrint.Value.Hour}:{item.ThoiGianPrint.Value.Minute}:{item.ThoiGianPrint.Value.Second}";
                    worksheet1.Cells[$"D{row}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet1.Cells[$"D{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet1.Cells[string.Format("E{0}", row)].Value = item.TenTaiLieuDinhKem;
                    worksheet1.Cells[$"E{row}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet1.Cells[$"E{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet1.Cells[string.Format("F{0}", row)].Value = item.TongSoTrangDaIn;
                    worksheet1.Cells[$"F{row}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet1.Cells[$"F{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    worksheet1.Cells[string.Format("G{0}", row)].Value =item.PaperSize;
                    worksheet1.Cells[$"G{row}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet1.Cells[$"G{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    worksheet1.Cells[string.Format("H{0}", row)].Value = item.TenMayIn;
                    worksheet1.Cells[$"H{row}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet1.Cells[$"H{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet1.Cells[string.Format("I{0}", row)].Value = item.TrangThaiText;
                    worksheet1.Cells[$"I{row}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet1.Cells[$"I{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    row++;
                }

                worksheet1.Cells[row, 1, row, 5].Merge = true;
                worksheet1.Cells[$"A{row}:E{row}"].Value = "Tổng trang đã in";
                worksheet1.Cells[$"A{row}:E{row}"].Style.Font.Bold = true;
                worksheet1.Cells[$"A{row}:E{row}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet1.Cells[$"A{row}:E{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet1.Cells[row, 6, row, 9].Merge = true;
                worksheet1.Cells[$"F{row}:I{row}"].Value = sum;
                worksheet1.Cells[$"F{row}:I{row}"].Style.Font.Bold = true;
                worksheet1.Cells[$"F{row}:I{row}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet1.Cells[$"F{row}:I{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

          

            string filename1 = DateTime.Now.ToString("MMddyyyyHHmm")+ convertToUnSign2(fileName);
            string targetpath1 = Server.MapPath("~/Content/Uploads/");
            FileInfo fi = new FileInfo(targetpath1 + filename1);
            package.SaveAs(fi);
            return Json(filename1, JsonRequestBehavior.AllowGet);
        }

        public string convertToUnSign2(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            sb = sb.Replace(" ", "");
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        public static double GetTrueColumnWidth(double width)
        {
            //DEDUCE WHAT THE COLUMN WIDTH WOULD REALLY GET SET TO
            double z = 1d;
            if (width >= (1 + 2 / 3))
            {
                z = Math.Round((Math.Round(7 * (width - 1 / 256), 0) - 5) / 7, 2);
            }
            else
            {
                z = Math.Round((Math.Round(12 * (width - 1 / 256), 0) - Math.Round(5 * width, 0)) / 12, 2);
            }

            //HOW FAR OFF? (WILL BE LESS THAN 1)
            double errorAmt = width - z;

            //CALCULATE WHAT AMOUNT TO TACK ONTO THE ORIGINAL AMOUNT TO RESULT IN THE CLOSEST POSSIBLE SETTING 
            double adj = 0d;
            if (width >= (1 + 2 / 3))
            {
                adj = (Math.Round(7 * errorAmt - 7 / 256, 0)) / 7;
            }
            else
            {
                adj = ((Math.Round(12 * errorAmt - 12 / 256, 0)) / 12) + (2 / 12);
            }

            //RETURN A SCALED-VALUE THAT SHOULD RESULT IN THE NEAREST POSSIBLE VALUE TO THE TRUE DESIRED SETTING
            if (z > 0)
            {
                return width + adj + 0.11;
            }

            return 0d;
        }

        public FilePathResult Download(string Name)
        {
           
            var url = Path.Combine(Server.MapPath("~/Content/uploads/"), Name);
            byte[] fileBytes = System.IO.File.ReadAllBytes(url);
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/Content/uploads"));
            System.IO.FileInfo[] fileNames = dir.GetFiles(Name);
            return File(url, "multipart/form-data", Name);
        }

        //public JsonResult filter()
        //{
        //    var data = (from bi in db.NV_BanIn
        //                group bi by bi.PaperSize into g
        //                select new { PaperSize  = g.Key})
        //                .ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
    }
}