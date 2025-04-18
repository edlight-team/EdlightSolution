﻿using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationServices.PermissionService
{
    public class PermissionImplementation : IPermissionService
    {
        private readonly List<RolesModel> roles;
        private readonly List<PermissionsModel> permissions;
        private readonly List<RolesPermissionsModel> rolesToPermissions;
        private readonly List<UsersRolesModel> usersToRoles;
        private UserModel currentUser;
        public PermissionImplementation()
        {
            roles = new();
            permissions = new();
            rolesToPermissions = new();
            usersToRoles = new();
        }

        public async Task ConfigureService(IWebApiService api, UserModel currentUser)
        {
            roles.Clear();
            permissions.Clear();
            rolesToPermissions.Clear();
            usersToRoles.Clear();

            roles.AddRange(await api.GetModels<RolesModel>(WebApiTableNames.Roles));
            permissions.AddRange(await api.GetModels<PermissionsModel>(WebApiTableNames.Permissions));
            rolesToPermissions.AddRange(await api.GetModels<RolesPermissionsModel>(WebApiTableNames.RolesPermissions));
            usersToRoles.AddRange(await api.GetModels<UsersRolesModel>(WebApiTableNames.UsersRoles));

            this.currentUser = currentUser;
        }
        public Task<bool> IsInRole(RolesModel role)
        {
            if (currentUser.Login == "admin") return Task.FromResult(true);
            foreach (UsersRolesModel item in usersToRoles)
            {
                if (item.IdUser == currentUser.ID && item.IdRole == role.Id)
                {
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }
        public Task<bool> IsInPermission(RolesModel role, PermissionsModel permission)
        {
            if (currentUser.Login == "admin") return Task.FromResult(true);
            foreach (RolesPermissionsModel item in rolesToPermissions)
            {
                if (item.IdRole == role.Id && item.IdPermission == permission.Id)
                {
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }
        public Task<bool> IsInPermission(string permissionName)
        {
            if (currentUser.Login == "admin") return Task.FromResult(true);

            UsersRolesModel userRole = usersToRoles.FirstOrDefault(r => r.IdUser == currentUser.ID);
            RolesModel role = roles.FirstOrDefault(r => r.Id == userRole.IdRole);

            PermissionsModel permission = permissions.FirstOrDefault(p => p.PermissionName == permissionName);
            if (permission == null) return Task.FromResult(false);

            foreach (RolesPermissionsModel item in rolesToPermissions)
            {
                if (item.IdRole == role.Id && item.IdPermission == permission.Id)
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
        public Task<RolesModel> GetRoleByName(string name) => Task.FromResult(roles.FirstOrDefault(r => r.RoleName.ToLower() == name.ToLower()));

    }
}
