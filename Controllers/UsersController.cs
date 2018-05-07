using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using securityfilter;
using securityservice.Model;
using securityservice.Services.Interfaces;

namespace securityservice.Controllers {
    [Route ("api/[controller]")]
    public class UsersController : Controller {
        private readonly IUserService _userService;

        public UsersController (IUserService userService) {
            _userService = userService;
        }

        [HttpGet]
        [ResponseCache (CacheProfileName = "accesscache")]
        [SecurityFilter ("user__allow_read")]
        public async Task<IActionResult> Get ([FromQuery] int startat, [FromQuery] int quantity, [FromQuery] string fieldFilter, [FromQuery] string fieldValue, [FromQuery] string orderField, [FromQuery] string order) {

            var fieldFilterEnum = UserFieldEnum.Default;
            Enum.TryParse (fieldFilter, true, out fieldFilterEnum);
            var orderFieldEnum = UserFieldEnum.Default;
            Enum.TryParse (orderField, true, out orderFieldEnum);
            var orderEnumValue = OrderEnum.Ascending;
            Enum.TryParse (order, true, out orderEnumValue);

            if (quantity == 0)
                quantity = 50;

            var (users, total) = await _userService.getUsers (startat, quantity, fieldFilterEnum, fieldValue, orderFieldEnum, orderEnumValue);

            return Ok (new { values = users, total = total });
        }

        [HttpGet ("list/")]
        [SecurityFilter ("user__allow_read")]
        [ResponseCache (CacheProfileName = "accesscache")]
        public async Task<IActionResult> GetList ([FromQuery] int[] userId) {
            var users = await _userService.getUsersById (userId);
            return Ok (users);
        }

        [HttpGet ("{id}")]
        [SecurityFilter ("user__allow_read")]
        [ResponseCache (CacheProfileName = "accesscache")]
        public async Task<IActionResult> Get (int id) {
            var user = await _userService.getUser (id);
            if (user == null)
                return NotFound ();

            return Ok (user);
        }

        [HttpPost]
        [SecurityFilter ("user__allow_update")]
        public async Task<IActionResult> Post ([FromBody] User user) {
            user.userId = 0;
            if (ModelState.IsValid) {
                user = await _userService.createUser (user);
                return Created ($"api/users/{user.userId}", user);
            }
            return BadRequest (ModelState);
        }

        [HttpPut ("{id}")]
        [SecurityFilter ("user__allow_update")]
        public async Task<IActionResult> Put (int id, [FromBody] User user) {
            if (ModelState.IsValid) {
                user = await _userService.updateUser (id, user);
                if (user == null && id != user.userId) {
                    return NotFound ();
                }
                return NoContent ();
            }
            return BadRequest (ModelState);
        }

        [HttpDelete ("{id}")]
        [SecurityFilter ("user__allow_update")]
        public async Task<IActionResult> Delete (int id) {
            var user = await _userService.deleteUser (id);
            if (user != null) {
                return NoContent ();
            }
            return NotFound ();
        }
    }
}