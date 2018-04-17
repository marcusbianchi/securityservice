using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using securityservice.Data;
using securityservice.Model;
using securityservice.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace securityservice.Services {
    public class GroupService : IGroupService {

        private readonly ApplicationDbContext _context;
        private readonly IPermisionService _permisionService;
        private readonly IUserService _userService;
        public GroupService (ApplicationDbContext context, IPermisionService permisionService, IUserService userService) {
            _context = context;
            _permisionService = permisionService;
            _userService = userService;
        }
        public async Task<UserGroup> createUserGroup (UserGroup userGroup) {
            userGroup.users = new List<User> ();
            userGroup.enabled = true;
            userGroup.permissions = new string[0];
            _context.UserGroups.Add (userGroup);
            await _context.SaveChangesAsync ();
            return userGroup;
        }

        public async Task<UserGroup> deleteUserGroup (int userGroupId) {
            var userGroup = await _context.UserGroups.Where (x => x.userGroupId == userGroupId).FirstOrDefaultAsync ();
            if (userGroup != null) {
                userGroup.enabled = false;
                _context.UserGroups.Update (userGroup);
                await _context.SaveChangesAsync ();
            }
            return userGroup;
        }

        public async Task<UserGroup> getGroup (int userGroupId) {
            var userGroup = await _context.UserGroups.Where (x => x.userGroupId == userGroupId)
                .Include (p => p.users).FirstOrDefaultAsync ();
            return userGroup;
        }

        public async Task<UserGroup> getUserByName (string name) {
            var userGroup = await _context.UserGroups.Where (x => x.name == name)
                .Include (p => p.users).FirstOrDefaultAsync ();
            return userGroup;
        }
        public async Task<UserGroup> updateUserGroup (int userGroupId, UserGroup userGroup) {
            var curUserGroup = await _context.UserGroups.Where (x => x.userGroupId == userGroupId)
                .Include (p => p.users).FirstOrDefaultAsync ();
            if (curUserGroup != null) {
                curUserGroup.name = userGroup.name;
                curUserGroup.description = userGroup.description;
                _context.Entry (curUserGroup).State = EntityState.Modified;
                await _context.SaveChangesAsync ();
            }
            return await getGroup (userGroupId);
        }
        public async Task<(List<UserGroup>, int)> getUserGroups (int startat, int quantity, GroupFieldEnum fieldFilter, string fieldValue, GroupFieldEnum orderField, OrderEnum order) {
            var queryUserGroups = _context.UserGroups.Where (x => x.enabled == true);
            queryUserGroups = ApplyFilter (queryUserGroups, fieldFilter, fieldValue);
            queryUserGroups = ApplyOrder (queryUserGroups, orderField, order);
            var users = await queryUserGroups.Include (x => x.users)
                .Skip (startat).Take (quantity).ToListAsync ();
            var queryUserGroupCount = _context.UserGroups.Where (x => x.enabled == true);
            queryUserGroupCount = ApplyFilter (queryUserGroupCount, fieldFilter, fieldValue);
            queryUserGroupCount = ApplyOrder (queryUserGroupCount, orderField, order);
            var totalCount = await queryUserGroupCount.CountAsync ();
            return (users, totalCount);
        }

        public async Task<List<UserGroup>> getUserGroupssById (int[] userGroupIds) {
            var userGroups = await _context.UserGroups.Where (x => userGroupIds.Contains (x.userGroupId))
                .Include (p => p.users).ToListAsync ();
            return userGroups;
        }

        public async Task<UserGroup> AddPermisionToUserGroup (int userGroupId, string permission) {
            var permissions = _permisionService.getPermisions ();
            if (permissions.ContainsKey (permission)) {
                var userGroup = await getGroup (userGroupId);
                userGroup.permissions = userGroup.permissions.Append (permission).ToArray ();
                _context.Entry (userGroup).State = EntityState.Modified;
                await _context.SaveChangesAsync ();
                return userGroup;
            }
            return null;

        }
        public async Task<UserGroup> RemovePermisionFromUserGroup (int userGroupId, string permission) {
            var userGroup = await getGroup (userGroupId);
            if (userGroup != null && userGroup.permissions.Contains (permission)) {
                userGroup.permissions = userGroup.permissions.Where (w => w != permission).ToArray ();
                _context.Entry (userGroup).State = EntityState.Modified;
                await _context.SaveChangesAsync ();
                return userGroup;
            }
            return null;
        }
        public async Task<UserGroup> AddUserToUserGroup (int userGroupId, User user) {
            user = await _userService.getUser (user.userId);
            if (user != null) {
                var userGroup = await getGroup (userGroupId);
                userGroup.users.Add (user);
                _context.Entry (userGroup).State = EntityState.Modified;
                await _context.SaveChangesAsync ();
                return userGroup;
            }
            return null;
        }

        public async Task<UserGroup> RemoveUserFromUserGroup (int userGroupId, User user) {
            var userGroup = await getGroup (userGroupId);
            user = await _userService.getUser (user.userId);
            if (userGroup != null && user != null && userGroup.users.Contains (user)) {
                userGroup.users.Remove (user);
                _context.Entry (userGroup).State = EntityState.Modified;
                await _context.SaveChangesAsync ();
                return userGroup;
            }
            return null;
        }

        private IQueryable<UserGroup> ApplyFilter (IQueryable<UserGroup> queryUserGroups,
            GroupFieldEnum fieldFilter, string fieldValue) {
            switch (fieldFilter) {
                case GroupFieldEnum.name:
                    queryUserGroups = queryUserGroups.Where (x => x.name.Contains (fieldValue));
                    break;
                case GroupFieldEnum.description:
                    queryUserGroups = queryUserGroups.Where (x => x.description.Contains (fieldValue));
                    break;
                default:
                    break;
            }
            return queryUserGroups;
        }

        private IQueryable<UserGroup> ApplyOrder (IQueryable<UserGroup> queryUserGroups,
            GroupFieldEnum orderField, OrderEnum order) {
            switch (orderField) {
                case GroupFieldEnum.name:
                    if (order == OrderEnum.Ascending)
                        queryUserGroups = queryUserGroups.OrderBy (x => x.name);
                    else
                        queryUserGroups = queryUserGroups.OrderByDescending (x => x.name);
                    break;
                case GroupFieldEnum.description:
                    if (order == OrderEnum.Ascending)
                        queryUserGroups = queryUserGroups.OrderBy (x => x.description);
                    else
                        queryUserGroups = queryUserGroups.OrderByDescending (x => x.description);
                    break;
                default:
                    queryUserGroups = queryUserGroups.OrderBy (x => x.name);
                    break;
            }
            return queryUserGroups;
        }
    }
}