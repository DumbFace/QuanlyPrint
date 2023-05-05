using QuanLyMayIn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyMayIn.Controllers
{
    [Authorize(Roles = "ADMIN, NHAN_VIEN")]
    public class HomeController : Controller
    {
        private Print_LimitEntities db = new Print_LimitEntities();
        private UserInfo user = new UserInfo();
        // GET: Home
        public ActionResult Index()
        {
            
            ViewBag.GioiHan = user.GioiHanBanIn();
            ViewBag.SoLuongBanIn = user.GetSoLuongBanInTrongThang(DateTime.Now.Month, DateTime.Now.Year);

            ViewBag.SoLuongDaIn = user.GetSoLuongBanDaInTrongThang(DateTime.Now.Month, DateTime.Now.Year);
            ViewBag.SoLuongChuaIn = user.GetSoLuongBanChuaInTrongThang(DateTime.Now.Month, DateTime.Now.Year);

            return View();
        }
    }
}