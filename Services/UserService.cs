using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using securityservice.Data;
using securityservice.Model;
using securityservice.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace securityservice.Services {
    public class UserService : IUserService {

        private readonly ApplicationDbContext _context;
        public UserService (ApplicationDbContext context) {
            _context = context;
        }
        public async Task<User> createUser (User user) {
            user.userGroup = null;
            user.enabled = true;
            _context.Users.Add (user);
            await _context.SaveChangesAsync ();
            return user;
        }

        public async Task<User> deleteUser (int userId) {
            var user = await _context.Users.Where (x => x.userId == userId).FirstOrDefaultAsync ();
            if (user != null) {
                user.enabled = false;
                _context.Users.Update (user);
                await _context.SaveChangesAsync ();

            }
            return user;
        }

        public async Task<User> getUser (int userId) {
            var user = await _context.Users.Where (x => x.userId == userId)
                .Include (p => p.userGroup).FirstOrDefaultAsync ();
            return user;
        }

        public async Task<User> getUserByName (string username, string password) {
            var user = await _context.Users.Where (x => x.username == username && x.password == password)
                .Where (x => x.enabled == true).Include (p => p.userGroup).FirstOrDefaultAsync ();
            return user;
        }

        public async Task<(List<User>, int)> getUsers (int startat, int quantity, UserFieldEnum fieldFilter, string fieldValue, UserFieldEnum orderField, OrderEnum order) {
            var queryUser = _context.Users.Where (x => x.enabled == true);
            queryUser = ApplyFilter (queryUser, fieldFilter, fieldValue);
            queryUser = ApplyOrder (queryUser, orderField, order);
            var users = await queryUser.Include (x => x.userGroup)
                .Skip (startat).Take (quantity).ToListAsync ();
            var queryUserCount = _context.Users.Where (x => x.enabled == true);
            queryUserCount = ApplyFilter (queryUserCount, fieldFilter, fieldValue);
            queryUserCount = ApplyOrder (queryUserCount, orderField, order);
            var totalCount = await queryUserCount.CountAsync ();
            return (users, totalCount);
        }

        public async Task<List<User>> getUsersById (int[] userIds) {
            var users = await _context.Users.Where (x => userIds.Contains (x.userId))
                .Include (p => p.userGroup).ToListAsync ();
            return users;
        }

        public async Task<User> updateUser (int userId, User user) {
            var curUser = await _context.Users.Where (x => x.userId == userId).FirstOrDefaultAsync ();
            if (curUser != null) {
                user.userGroup = curUser.userGroup;
                _context.Entry (curUser).CurrentValues.SetValues (user);
                _context.SaveChanges ();
            }
            return await getUser (userId);
        }

        private IQueryable<User> ApplyFilter (IQueryable<User> queryUsers,
            UserFieldEnum fieldFilter, string fieldValue) {
            switch (fieldFilter) {
                case UserFieldEnum.email:
                    queryUsers = queryUsers.Where (x => x.email.Contains (fieldValue));
                    break;
                case UserFieldEnum.username:
                    queryUsers = queryUsers.Where (x => x.username.Contains (fieldValue));
                    break;
                default:
                    break;
            }
            return queryUsers;
        }

        private IQueryable<User> ApplyOrder (IQueryable<User> queryUsers,
            UserFieldEnum orderField, OrderEnum order) {
            switch (orderField) {
                case UserFieldEnum.email:
                    if (order == OrderEnum.Ascending)
                        queryUsers = queryUsers.OrderBy (x => x.email);
                    else
                        queryUsers = queryUsers.OrderByDescending (x => x.email);
                    break;
                case UserFieldEnum.username:
                    if (order == OrderEnum.Ascending)
                        queryUsers = queryUsers.OrderBy (x => x.username);
                    else
                        queryUsers = queryUsers.OrderByDescending (x => x.username);
                    break;
                default:
                    queryUsers = queryUsers.OrderBy (x => x.username);
                    break;
            }
            return queryUsers;
        }
    }
}