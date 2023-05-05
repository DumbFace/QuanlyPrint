using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyMayIn
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "DM_NhomNhanVien",
             url: "nhom-nhan-vien/{action}/{id}",
             defaults: new { controller = "DM_NhomNhanVien", action = "Index", id = UrlParameter.Optional }
         );

        routes.MapRoute(
           name: "NV_BaoCaoXemChiTiet",
           url: "bao-cao/xem-chi-tiet/{id}",
           defaults: new { controller = "NV_BaoCao", action = "XemChiTiet", id = UrlParameter.Optional }
        );

            routes.MapRoute(
           name: "NV_BaoCaoXemChiTietNhom",
           url: "bao-cao/xem-chi-tiet-nhom/{id}",
           defaults: new { controller = "NV_BaoCao", action = "XemChiTietNhom", id = UrlParameter.Optional }
        );


            routes.MapRoute(
        name: "NV_BaoCaoIndexmayin",
        url: "bao-cao/may-in/{id}",
        defaults: new { controller = "NV_BaoCao", action = "MayIn", id = UrlParameter.Optional }
    );
            routes.MapRoute(
      name: "NV_BaoCaoIndexnhom",
      url: "bao-cao/nhom/{id}",
      defaults: new { controller = "NV_BaoCao", action = "Nhom", id = UrlParameter.Optional }
  );
            routes.MapRoute(
           name: "NV_BaoCaoIndex",
           url: "bao-cao/{action}/{id}",
           defaults: new { controller = "NV_BaoCao", action = "Index", id = UrlParameter.Optional }
       );
          

            routes.MapRoute(
            name: "DM_NhanVien_soluongbanin",
            url: "so-luong-ban-in/{action}/{id}",
            defaults: new { controller = "DM_NhanVien", action = "SoLuongBanIn", id = UrlParameter.Optional }
        );

            routes.MapRoute(
               name: "DM_NhanVien",
               url: "nhan-vien/{action}/{id}",
               defaults: new { controller = "DM_NhanVien", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "DM_MayIn",
               url: "may-in/{action}/{id}",
               defaults: new { controller = "DM_MayIn", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
             name: "DM_ChiTietNhanVienMayIn",
             url: "nhan-vien-may-in/{action}/{id}",
             defaults: new { controller = "DM_ChiTietNhanVienMayIn", action = "Index", id = UrlParameter.Optional }
         );

            routes.MapRoute(
                 name: "NV_BanIn_XemTruoc",
                 url: "ban-in/xem-truoc-ban-in/{id}",
                 defaults: new { controller = "NV_BanIn", action = "BanIn", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "NV_BanIn",
                url: "ban-in/{action}/{id}",
                defaults: new { controller = "NV_BanIn", action = "Index", id = UrlParameter.Optional }
            );   


            routes.MapRoute(
                name: "DM_NhomTaiKhoanIndex",
                url: "nhom-tai-khoan/{action}/{id}",
                defaults: new { controller = "DM_NhomTaiKhoan", action = "Index", id = UrlParameter.Optional }
             );

            routes.MapRoute(
                name: "HT_ChiTietPhanQuyenIndex",
                url: "phan-quyen/{action}/{id}",
                defaults: new { controller = "HT_ChiTietPhanQuyen", action = "Index", id = UrlParameter.Optional }
            );
    
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
