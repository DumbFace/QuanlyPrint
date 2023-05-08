using FluentEmail.Core;
using FluentEmail.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Security;
using System.Threading.Tasks;

using System.Net;

using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Entity;
using QuanLyMayIn.Models;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Text;

namespace QuanLyMayIn.Controllers
{


    public class AccountController : Controller
    {
        private Print_LimitEntities db = new Print_LimitEntities();
        [AllowAnonymous]


        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public JsonResult Login(LoginViewModel model, string returnUrl)
        {
            model.RememberMe = true;
            ViewBag.returnUrl = returnUrl;

            var tk = db.DM_NhanVien.Where(s => s.TenTaiKhoan == model.UserName && s.MatKhau == model.Password).SingleOrDefault();

            if (tk == null)
            {
                var tk1 = db.DM_NhanVien.Single(s => s.ID_NhanVien == -1);
                return Json(tk1, JsonRequestBehavior.AllowGet);
            }

            var ntk = db.DM_NhomTaiKhoan.Where(_ => _.KeyNhomTaiKhoan == tk.KeyNhomTaiKhoan).Single();

            var str = $"{tk.ID_NhanVien},{tk.TenNhanVien},{ntk.KeyNhomTaiKhoan},{tk.TenTaiKhoan}";

            if (model.RememberMe)
            {
                var authTicket = new FormsAuthenticationTicket(2, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(525600),
                false, str);

                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
                {
                    HttpOnly = true,
                    Expires = authTicket.Expiration
                };

                Response.AppendCookie(authCookie);
            }
            else
            {
                var authTicket = new FormsAuthenticationTicket(2, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(30),
                false, str);

                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
                {
                    HttpOnly = true,
                };
                Response.AppendCookie(authCookie);
            }

            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Url.Action("Index", "Home"), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account"); ;
        }

        public async Task sendmail(string emailNG, string code, string Ten)
        {
            var sender = new SmtpSender(() => new System.Net.Mail.SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential("hoanganhhai123123@gmail.com", "tsjtxwhofencdfuf"),
                EnableSsl = true,

            });

            Email.DefaultSender = sender;

            var email = Email.From("hoang.nm.0608099@gmail.com", "VINAAI COMPANY")
                             .To(emailNG)
                             .Subject("YÊU CẦU THAY ĐỔI MẬT KHẨU KIỂM SOÁT TÀI SẢN")
                             .Body($"<div><div><img src = 'https://www.vinaai.com/logo/logo.png' width = '350px'/></div><div style ='width:100%;height:2px;background-color:grey;margin-top:1rem'></div><div style ='font-size: 16px;'><p style = 'color:black'>Xin chào<strong> {Ten}!<strong></strong></strong></p><p style = 'color:black'>Cảm ơn {Ten} đã tin tưởng và sử dụng dịch vụ từ <a href ='https://www.vinaai.com/'>VinaAI</a></p><p style = 'color:black'>Mã code của bạn là: <strong style = 'color:black;'>{code}</strong></p><span style = 'color:black'>Mã code này chỉ<strong style = 'color:black'> sử dụng một lần </strong></p><p style = 'color:black'>Không được chia sẽ mã này với bất kì ai</p><p style = 'color:black'>Mọi thắc mắc xin vui lòng liên hệ thông tin dưới đây!</p></div><div style = 'width:100%;height:2px;background-color:grey;margin-top:1rem'></div> ", true);
            try
            {
                await email.SendAsync();
            }
            catch
            {

            }
        }
        //Logout Dùng với Button Longout ở index
        public JsonResult ResetMatKhau([Bind(Include = "SoDienThoai")] DM_NhanVien canbo)
        {
            string m;
            Random r = new Random();
            m = r.Next(1000, 9999).ToString();
            var timcanbo = db.DM_NhanVien.Where(_ => _.SoDienThoai == canbo.SoDienThoai).FirstOrDefault();

            if (timcanbo == null)
            {
                return Json(new { status = false, message = "Email này chưa được đăn kí, vui lòng kiểm tra lại" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var findtk = db.DM_NhanVien.Single(_ => _.ID_NhanVien == timcanbo.ID_NhanVien);
                var editTK = db.DM_NhanVien.Find(findtk.ID_NhanVien);
                if (editTK != null)
                {
                    editTK.Code = m;
                    db.Entry(editTK).State = EntityState.Modified;
                    db.SaveChanges();
                    Task t1 = sendmail(timcanbo.SoDienThoai, m, timcanbo.TenNhanVien);
                    return Json(new { status = true, IDTK = editTK.ID_NhanVien, message = "Xin vui lòng nhập mã xác thực và mật khẩu mới" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Email của bạn chưa cấp tài khoản, vui lòng kiểm tra lại " }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult EditMatKhau(string TenTaiKhoan, string Code, string MatKhau, string XNMatKhau)
        {
            try
            {
                var tk = db.DM_NhanVien.Single(_ => _.TenTaiKhoan == TenTaiKhoan);
                if (tk == null)
                {
                    return Json(new { status = false, message = "Không tìm thấy tài khoản, vui lòng kiểm tra lại" }, JsonRequestBehavior.AllowGet);
                }
                else if (Code != tk.Code)
                {
                    return Json(new { status = false, message = "Mã code nhập không đúng, vui lòng kiểm tra lại email" }, JsonRequestBehavior.AllowGet);
                }
                else if (MatKhau != XNMatKhau)
                {
                    return Json(new { status = false, message = "Mật Khẩu xác nhận không đúng, vui lòng kiểm tra lại." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tk.MatKhau = MatKhau;
                    db.Entry(tk).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { status = true, message = "Thay đổi mật khẩu thành công." }, JsonRequestBehavior.AllowGet); ;
                }
            }
            catch
            {
                return Json(new { status = false, message = "Không tìm thấy tài khoản, vui lòng kiểm tra lại" }, JsonRequestBehavior.AllowGet);
            }
        }

    }

}