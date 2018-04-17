using System.Collections.Generic;
using System.Threading.Tasks;
using securityservice.Model;

namespace securityservice.Services.Interfaces
{
    public interface IGroupService {
        Task<(List<UserGroup>, int)> getUserGroups (int startat, int quantity, GroupFieldEnum fieldFilter, string fieldValue, GroupFieldEnum orderField, OrderEnum order);
        Task<UserGroup> getGroup (int userGroupId);
        Task<UserGroup> getUserByName (string name);
        Task<List<UserGroup>> getUserGroupssById (int[] userGroupIds);
        Task<UserGroup> createUserGroup (UserGroup userGroup);
        Task<UserGroup> updateUserGroup (int userGroupId, UserGroup userGroup);
        Task<UserGroup> deleteUserGroup (int userGroupId);
        Task<UserGroup> AddUserToUserGroup (int userGroupId, User user);
        Task<UserGroup> RemoveUserFromUserGroup (int userGroupId, User user);
        Task<UserGroup> AddPermisionToUserGroup (int userGroupId, string permission);
        Task<UserGroup> RemovePermisionFromUserGroup (int userGroupId, string permission);

    }
}