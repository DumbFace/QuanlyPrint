//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyMayIn.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DM_NhanVien
    {
        public int ID_NhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string TenTaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string Bios_MayTinh { get; set; }
        public string KeyNhomTaiKhoan { get; set; }
        public string KeyNhomNhanVien { get; set; }
        public Nullable<int> SoLuongBanInTrongThang { get; set; }
        public string Code { get; set; }
        public Nullable<bool> InVoHan { get; set; }
    }
}
