using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using QuanLyMayIn.Models;
namespace QuanLyMayIn.Models
{
  
    public class UserInfo
    {
        private Print_LimitEntities db = new Print_LimitEntities();

        public UserInfo()
        {

        }

        public int GetUser()
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string str = authTicket.UserData;
            string[] subs = str.Split(',');
            return Int32.Parse(subs[0]);

        }

        public string GetTaiKhoan()
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string str = authTicket.UserData;
            string[] subs = str.Split(',');
            return subs[3];

        }

        public string GetUserName()
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string str = authTicket.UserData;
            string[] subs = str.Split(',');
            return subs[1];

        }

        public string GetGroupKey()
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string str = authTicket.UserData;
            string[] subs = str.Split(',');
            return subs[2];

        }

        public int GetSoLuongBanInTrongThang(int thang, int nam)
        {
            var ID = GetUser();
            var data = db.NV_BanIn.Where(_ =>_.ID_NhanVien == ID && _.ThoiGianPrint.Value.Year == nam && _.ThoiGianPrint.Value.Month == thang);
            if(data.Count() != 0)
            {
                return (int)data.Sum(_ => _.TongSoTrangDaIn);
            }
            return 0;
        }
        
        public int GetSoLuongBanDaInTrongThang(int thang, int nam)
        {
            var ID = GetUser();
            var data = db.NV_BanIn.Where(_ =>_.ID_NhanVien == ID && _.ThoiGianPrint.Value.Year == nam && _.ThoiGianPrint.Value.Month == thang && _.TrangThai == true);
            if(data.Count() != 0)
            {
                return (int)data.Sum(_ => _.TongSoTrangDaIn);
            }
            return 0;
        }
        public int GetSoLuongBanChuaInTrongThang(int thang, int nam)
        {
            var ID = GetUser();
            var data = db.NV_BanIn.Where(_ =>_.ID_NhanVien == ID && _.ThoiGianPrint.Value.Year == nam && _.ThoiGianPrint.Value.Month == thang && _.TrangThai == false);
            if(data.Count() != 0)
            {
                return (int)data.Sum(_ => _.TongSoTrangDaIn);
            }
            return 0;
        }

        public int GioiHanBanIn()
        {
            var ID = GetUser();
            var checkLoaiNhanVien = db.DM_NhanVien.Where(_ => _.ID_NhanVien == ID).FirstOrDefault();
            if(checkLoaiNhanVien.InVoHan == true)
            {
                return -1;
            }
            else
            {
                 if (checkLoaiNhanVien.KeyNhomNhanVien == "DEFALT")
            {
                int SoLuongBanIn = (int)checkLoaiNhanVien.SoLuongBanInTrongThang;
                return SoLuongBanIn;
            }
            else
            {
                var SoLuongBanIn = (int)db.DM_NhomNhanVien.Where(_ => _.KeyNhomNhanVien == checkLoaiNhanVien.KeyNhomNhanVien).Select(_ => _.SoLuongBanInTrongThang).Single();
                return SoLuongBanIn;
            }
            }
           
        }

        public bool CheckQuyen(string menuCode, string ChucNangCode)
        {
            var user = GetUser();
            var tk = GetTaiKhoan(); 
            var ntk = db.DM_NhanVien.FirstOrDefault(_ => _.TenTaiKhoan == tk).KeyNhomTaiKhoan;

            var data = db.HT_ChiTietPhanQuyen.Where(_ => _.KeyNhomTaiKhoan == ntk && _.TrangThai == true && _.KeyChucNang.ToUpper() == ChucNangCode.ToUpper()).Select(_ => _.KeyMenu);

            return !data.Contains(menuCode);
        }
    }
}