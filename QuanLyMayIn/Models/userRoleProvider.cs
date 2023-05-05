using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using QuanLyMayIn.Models;
namespace QuanLyMayIn.Models
{
    public class userRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            Print_LimitEntities db = new Print_LimitEntities();

            string[] role = null;

            role = (from tk in db.DM_NhanVien
                    join ntk in db.DM_NhomTaiKhoan on tk.KeyNhomTaiKhoan equals ntk.KeyNhomTaiKhoan
                    where tk.TenTaiKhoan == username
                    select ntk.KeyNhomTaiKhoan).ToArray();
            return role;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}