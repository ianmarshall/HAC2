﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HAC.Infrastructure
{
    public class CustomRoleProvider : RoleProvider
    {
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
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
            //using (DatabaseEntities db = new DatabaseEntities())
            //{
            //    User user = db.Users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase) || u.Email.Equals(username, StringComparison.CurrentCultureIgnoreCase));

            //    var roles = from ur in user.UserRoles
            //                from r in db.Roles
            //                where ur.RoleId == r.Id
            //                select r.Name;
            //    if (roles != null)
            //        return roles.ToArray();
            //    else
                    return new string[] { "Admin" }; ;
            //}
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
           return true;
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