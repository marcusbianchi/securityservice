using System.Collections.Generic;
using System.Threading.Tasks;
using securityservice.Model;

namespace securityservice.Services.Interfaces
{
    public interface IUserService {
        Task<(List<User>, int)> getUsers (int startat, int quantity, UserFieldEnum fieldFilter, string fieldValue, UserFieldEnum orderField, OrderEnum order);
        Task<User> getUser (int userId);
        Task<User> getUserByName (string username, string password);
        Task<List<User>> getUsersById (int[] userIds);
        Task<User> createUser (User user);
        Task<User> updateUser (int userId, User user);
        Task<User> deleteUser (int userId);

    }
}